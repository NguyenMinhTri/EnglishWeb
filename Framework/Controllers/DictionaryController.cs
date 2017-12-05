﻿using Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Service.Admin;
using Framework.Common;
using System.Threading;
using System.Web.UI;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Framework.ViewModels;
using Framework.Service.Client;
using Framework.Model;
using Framework.Model.Google;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Framework.Model.Bot;
using Microsoft.AspNet.Identity;
using Framework.ViewData.Admin.GetData;

namespace Framework.Controllers
{

    public class DictionaryController : LayoutController
    {
        IClientDictionaryService _clientDictionaryService;
        IOurWordService _ourWordService;
        IDetailOurWordService _detailOutWordService;
        public DictionaryController(ILayoutService layoutService,
            IClientDictionaryService clientDictionaryService,
            IOurWordService ourWordService,
            IDetailOurWordService detailOutWordService
            )
            : base(layoutService)
        {
            _clientDictionaryService = clientDictionaryService;
            _ourWordService = ourWordService;
            _detailOutWordService = detailOutWordService;
        }


        DictionariesViewModel DictionariesViewModel
        {
            get
            {
                return (DictionariesViewModel)_viewModel;
            }
        }

        OldWordsViewModel OldWordsViewModel
        {
            get
            {
                return (OldWordsViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option)
        {
            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "dictionaries":
                        {
                            _viewModel = new DictionariesViewModel();
                            CreateLayoutView("Từ điển");
                            break;
                        }
                    case "oldwords":
                        {
                            _viewModel = new OldWordsViewModel();
                            CreateLayoutView("Từ đã tra");
                            break;
                        }
                    default:
                        {
                            _viewModel = new DictionariesViewModel();
                            CreateLayoutView("Từ điển");
                            break;
                        }
                }
            }
            else
            {
                _viewModel = new DictionariesViewModel();
                CreateLayoutView("Từ điển");
            }

            return View(_viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> Dictionaries(string keyword)
        {
            OxfordDict dict = new OxfordDict();
            GoogleTrans googleTransJson = new GoogleTrans();
            keyword = keyword.Trim();
            _viewModel = new DictionariesViewModel();
            bool downloadSucceeded = false;
            if (keyword != "")
            {
                int size = keyword.Split(' ').Length;
                if (size > 1)
                {
                    googleTransJson = await _clientDictionaryService.startGoogleTrans(keyword);
                    DictionariesViewModel.isGoogleTrans = true;
                    DictionariesViewModel.m_GoogleTrans = googleTransJson;
                }
                else
                {
                    try
                    {
                        dict = await _clientDictionaryService.startCrawlerOxford(keyword);
                        downloadSucceeded = true;
                    }
                    catch(Exception e)
                    {
                        downloadSucceeded = false;

                    }
                    if (!downloadSucceeded)
                    {
                        googleTransJson = await _clientDictionaryService.startGoogleTrans(keyword);
                        DictionariesViewModel.isGoogleTrans = true;
                        DictionariesViewModel.m_GoogleTrans = googleTransJson;
                    }
                        
                    DictionariesViewModel.m_Explanation = dict.m_Explanation;
                    DictionariesViewModel.m_SoundUrl = dict.m_SoundUrl;
                    DictionariesViewModel.m_Type = dict.m_Type;
                    DictionariesViewModel.m_Voca = dict.m_Voca;
                    DictionariesViewModel.m_Pron = dict.m_Pron.Replace("BrE","");
                }
                DictionariesViewModel.m_ExaTraCau = await _clientDictionaryService.startCrawlerTraCau(keyword);
            }
            return PartialView("_Dictionaries", DictionariesViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Tick(OurWordViewModel ourword)
        {
            GoogleTrans detailVietnamese = new GoogleTrans();
            detailVietnamese = await _clientDictionaryService.startGoogleDetailTrans(ourword.Word);

            string meanVN = detailVietnamese.dict.First().pos + ": ";
            foreach (var item in detailVietnamese.dict.First().terms)
            {
                meanVN += item + ", ";
            }

            OurWord newOurWord = new OurWord();
            DetailOurWord detailWord = new DetailOurWord();
            FieldHelper.CopyNotNullValue(newOurWord, ourword);
            newOurWord.MeanVi = meanVN;
            if (ourword != null)
            {

                var userId = User.Identity.GetUserId();
                ApplicationUser user = _service.GetUserById(userId);
                //Add

                _ourWordService.Add(newOurWord);
                _ourWordService.Save();
                detailWord.Id_User = userId;
                detailWord.Id_Messenger = user.Id_Messenger;
                detailWord.Id_OurWord = newOurWord.Id;
                detailWord.Learned = 1;
                detailWord.Id = 0;
                detailWord.Schedule = DateTime.Now;
                detailWord.UpdatedDate = DateTime.Now;
                // detailWord.
                try
                {
                    //detailWord.Schedule = new DateTime(0, 0, 0, 1, 0, 0);
                }
                catch
                {

                }
                try
                {
                    _detailOutWordService.Add(detailWord);
                    _detailOutWordService.Save();
                }
                catch (Exception e)
                {

                }

                //_ourWordService.addDictfavorite(newOurWord, user);
                //Add
                //_ourWordService.Add(newOurWord);

                //Update
                //newOurWord = _ourWordService.GetById(1);
                //FieldHelper.CopyNotNullValue(newOurWord, ourword);
                //_ourWordService.Update(newOurWord);

                //Delete
                //newOurWord = _ourWordService.GetById(1);
                //_ourWordService.Delete(newOurWord);

                try
                {
                    // _ourWordService.Save();
                    return Json(new
                    {
                        result = "success"
                    });
                }
                catch (Exception e)
                {
                    return Json(new
                    {
                        result = "failed",
                    });
                }
            }
            return Json(new
            {
                result = "failed"
            });
        }

        [HttpGet]
        public ActionResult OldWords()
        {
            _viewModel = new OldWordsViewModel();
            return PartialView("_OldWords", OldWordsViewModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> callChatBot(string contain, string id, string userid)
        {

            //post


            //
            contain = contain.ToLower().Trim();
            OxfordDict dict = new OxfordDict();
            GoogleTrans googleTransJson = new GoogleTrans();
            GoogleTrans detailVietnamese = new GoogleTrans();
            //Explain main of letter
            Message3 messExplaintion = new Message3();
            //Pron
            Message3 messPron = new Message3();
            //Vietnamese
            Message3 messVietnamese = new Message3();
            //
            int size = contain.Split(' ').Length;
            if (size > 1)
            {
                googleTransJson = await _clientDictionaryService.startGoogleTrans(contain);
                messExplaintion.text = googleTransJson.sentences[0].trans;
            }
            else
            {
                
                try
                {
                    dict = await _clientDictionaryService.startCrawlerOxford(contain);
                    messExplaintion.text = dict.m_Explanation.First().m_UseCase;
                }
                catch
                {
                    messExplaintion.text = contain;
                }
                messPron.text = dict.m_Pron.Replace("BrE","");
                detailVietnamese = await _clientDictionaryService.startGoogleDetailTrans(contain);

                messVietnamese.text = detailVietnamese.dict.First().pos + ": ";
                foreach( var item in detailVietnamese.dict.First().terms)
                {
                    messVietnamese.text +=item + ", ";
                }

            }
             
            RootObject2 hello = new RootObject2();
            Message2 sound = new Message2();
            Attachment2 attach = new Attachment2();
            Payload2 payload = new Payload2();
            sound.attachment.payload.url = dict.m_SoundUrl;
            sound.attachment.type = "audio";
            hello.messages.Add(messPron);
            hello.messages.Add(messVietnamese);
            hello.messages.Add(messExplaintion);
            hello.messages.Add(sound);

            return JsonConvert.SerializeObject(hello);
        }
        [AllowAnonymous]
        [HttpPost]
        public string notifyMessenger()
        {
            //  var listUser = _detailOutWordService.
            ListUserNofity listUserNotify = new ListUserNofity();
            List<ApplicationUser> listUser = _service.listUserID();
            List<int> listIDWord = new List<int>();
           
            foreach (var userDetail in listUser)
            {
                RemindUser reminderUser = new RemindUser();
                reminderUser.IdMess = userDetail.Id_Messenger;
                listIDWord = _detailOutWordService.listIdOutWord(userDetail.Id);
                //Update thoi gian 

                //
                if(listIDWord.Count !=0 )
                {
                    int i = 0;
                    foreach(var idWord in listIDWord)
                    {
                        //Gioi han 5 tu
                        if (i >= 5)
                            break;
                        VocaInfo vocaInfo = new VocaInfo();
                        var tempWord = _ourWordService.GetById(idWord);
                        vocaInfo.voca = tempWord.Word;
                        vocaInfo.pron = tempWord.Pronounciation;
                        vocaInfo.usecase = tempWord.MeanEn;
                        vocaInfo.meanVN = tempWord.MeanVi;
                        reminderUser.vocainfo.Add(vocaInfo);
                        i++;
                    }
                    listUserNotify.reminduser.Add(reminderUser);
                }
            }
            return JsonConvert.SerializeObject(listUserNotify);
        }

    }
}
