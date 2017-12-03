using Framework.Service;
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

namespace Framework.Controllers
{

    public class DictionaryController : LayoutController
    {
        IClientDictionaryService _clientDictionaryService;
        IOurWordService _ourWordService;

        public DictionaryController(ILayoutService layoutService,
            IClientDictionaryService clientDictionaryService,
            IOurWordService ourWordService
            )
            : base(layoutService)
        {
            _clientDictionaryService = clientDictionaryService;
            _ourWordService = ourWordService;
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
            GoogleTransJson googleTransJson = new GoogleTransJson();
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
        public JsonResult Tick(OurWordViewModel ourword)
        {
            if (ourword != null)
            {
                OurWord newOurWord = new OurWord();

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
                    _ourWordService.Save();
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
        public async Task<string> hello(string returnUrl)
        {
            returnUrl = returnUrl.ToLower();
            OxfordDict dict = new OxfordDict();
            //Contain
            Message3 check = new Message3();
            int size = returnUrl.Split(' ').Length;
            if (size > 1)
            {
                GoogleTransJson googleTransJson = await _clientDictionaryService.startGoogleTrans(returnUrl);
                check.text = googleTransJson.sentences[0].trans;
            }
            else
            {
                dict = await _clientDictionaryService.startCrawlerOxford(returnUrl);
                check.text = dict.m_Explanation.First().m_UseCase;
            }
            
            RootObject2 hello = new RootObject2();
            Message2 message = new Message2();
            Attachment2 attach = new Attachment2();
            Payload2 payload = new Payload2();
            message.attachment.payload.url = dict.m_SoundUrl;
            message.attachment.type = "audio";

            hello.messages.Add(check);
            hello.messages.Add(message);
            return JsonConvert.SerializeObject(hello);
        }
    }
}
