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
using Microsoft.AspNet.Identity;
using Framework.ViewData.Admin.GetData;
using System.Net.Http;
using System.Net;
using Framework.SignalR;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Controllers
{

    public class DictionaryController : LayoutController
    {

        IClientDictionaryService _clientDictionaryService;
        IDetailOurWordService _detailOutWordService;
        IDictCacheService _dictCache;
        public DictionaryController(ILayoutService layoutService,
            IClientDictionaryService clientDictionaryService,
            IDetailOurWordService detailOutWordService,
            IDictCacheService dictCache
            )
            : base(layoutService)
        {
            _clientDictionaryService = clientDictionaryService;
            _detailOutWordService = detailOutWordService;
            _dictCache = dictCache;
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
                            CreateLayoutView("Danh sách từ yêu thích");
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
            bool isVN = false;
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
                    catch (Exception e)
                    {
                        downloadSucceeded = false;

                    }
                    if (!downloadSucceeded)
                    {
                        googleTransJson = await _clientDictionaryService.startGoogleTrans(keyword);
                        if (googleTransJson.src == "vi")
                            isVN = true;
                        DictionariesViewModel.isGoogleTrans = true;
                        DictionariesViewModel.m_GoogleTrans = googleTransJson;
                    }
                    try
                    {
                        GoogleTrans detailVietnamese = new GoogleTrans();
                        detailVietnamese = await _clientDictionaryService.startGoogleDetailTrans(keyword);
                        if (detailVietnamese.src == "vi")
                            isVN = true;
                        string meanVN = detailVietnamese.dict.First().pos + ": ";
                        foreach (var item in detailVietnamese.dict.First().terms)
                        {
                            meanVN += item + ", ";
                        }
                        DictionariesViewModel.m_MeanVn = meanVN;
                    }
                    catch
                    {

                    }
                    DictionariesViewModel.m_Explanation = dict.m_Explanation;
                    DictionariesViewModel.m_SoundUrl = dict.m_SoundUrl;
                    DictionariesViewModel.m_Type = dict.m_Type;
                    DictionariesViewModel.m_Voca = dict.m_Voca;
                    DictionariesViewModel.m_Pron = dict.m_Pron.Replace("BrE", "");
                }
                DictionariesViewModel.m_ExaTraCau = await _clientDictionaryService.startCrawlerTraCau(keyword);
            }
            try
            {
                DictCache dictCache = _dictCache.findWordCache(keyword);
                if (dictCache == null)
                {
                    dictCache = new DictCache();
                    dictCache.VocaID = keyword;
                    dictCache.Pron = DictionariesViewModel.m_Pron;
                    dictCache.MeanEn = DictionariesViewModel.m_Explanation.FirstOrDefault().m_UseCase;
                    dictCache.MeanVi = DictionariesViewModel.m_MeanVn;
                    dictCache.SoundUrl = DictionariesViewModel.m_SoundUrl;
                    _dictCache.Add(dictCache);
                    _dictCache.Save();
                }
                else
                {
                    if (_detailOutWordService.findDetailOurWord(User.Identity.GetUserId(), dictCache.Id) != null)
                    {
                        DictionariesViewModel.love = true;
                    }
                }
            }
            catch (Exception e)
            {

            }
            return PartialView("_Dictionaries", DictionariesViewModel);
        }
        [HttpPost]
        //Save word
        public async Task<JsonResult> saveWord(string Voca)
        {
            string userId = User.Identity.GetUserId();


            DictCache dictCache = _dictCache.findWordCache(Voca);
            if(dictCache == null)
            {
                return Json(new
                {
                    result = "False",
                });
            }
            DetailOurWord detailWord = _detailOutWordService.findDetailOurWord(userId, dictCache.Id);
            //tick từ mới thành công
            if (detailWord == null)
            {
                ApplicationUser user = _service.GetUserById(userId);
                detailWord = new DetailOurWord();
                detailWord.Id_User = userId;
                detailWord.Id_Messenger = user.Id_Messenger;
                detailWord.Id_OurWord = dictCache.Id;
                detailWord.Learned = 1;
                detailWord.Id = 0;
                detailWord.Schedule = DateTime.Now.AddDays(-1);
                _detailOutWordService.Add(detailWord);
                _detailOutWordService.Save();
                await notifyMessenger();
                return Json(new
                {
                    result = "True"
                });
            }
            else
            {
                //tick cái nữa thì xóa tick
                _detailOutWordService.Delete(detailWord);
                _detailOutWordService.Save();
                return Json(new
                {
                    result = "False"
                });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Tick(OurWordViewModel data)
        {
            if (data != null)
            {
                DictCache dictCache = _dictCache.findWordCache(data.VocaID);

                if (dictCache == null)
                {
                    dictCache = new DictCache();
                    data.MeanVi = HttpUtility.HtmlDecode(data.MeanVi).ToString();
                    FieldHelper.CopyNotNullValue(dictCache, data);
                    try
                    {
                        _dictCache.Add(dictCache);
                        _dictCache.Save();
                    }
                    catch (Exception e)
                    {
                        return Json(new
                        {
                            result = "failed",
                        });
                    }
                }
                var userId = User.Identity.GetUserId();

                DetailOurWord detailWord = _detailOutWordService.findDetailOurWord(userId, dictCache.Id);

                if (detailWord == null)
                {
                    ApplicationUser user = _service.GetUserById(userId);
                    detailWord = new DetailOurWord();
                    detailWord.Id_User = userId;
                    detailWord.Id_Messenger = user.Id_Messenger;
                    detailWord.Id_OurWord = dictCache.Id;
                    detailWord.Learned = 1;
                    detailWord.Id = 0;
                    detailWord.Schedule = DateTime.Now.AddDays(-1);
                    _detailOutWordService.Add(detailWord);
                    
                }
                else
                {
                    _detailOutWordService.Delete(detailWord);
                }
                try
                {
                    _detailOutWordService.Save();
                    await notifyMessenger();
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
        public PartialViewResult OldWords()
        {
            _viewModel = new OldWordsViewModel();
            var listIDWord = _detailOutWordService.listOutWord(User.Identity.GetUserId(), -1);
            foreach (var idWord in listIDWord)
            {
                OldWordViewModelItem item = new OldWordViewModelItem();
                var tempWord = _dictCache.GetById(idWord.Id_OurWord);
                var infoVoca = _dictCache.findWordCache(tempWord.VocaID);

                item.Id = tempWord.Id;
                item.Learned = idWord.Learned;
                item.m_Voca = tempWord.VocaID;
                OldWordsViewModel.ListOldWords.Add(item);
            }
            return PartialView("_OldWords", OldWordsViewModel);
        }
        [AllowAnonymous]
        public async Task<string> callChatBot(string contain, string id, string userid)
        {
            contain = contain.ToString().ToLower().Trim();
            //Tim trong cache nếu ko có thì request crawler
            var dictTemp = _dictCache.findWordCache(contain);
            //post
            //
            bool isSaveCached = true;
            DictCache cacheDict = new DictCache() ;
            OxfordDict dict = new OxfordDict();
            GoogleTrans googleTransJson = new GoogleTrans();
            GoogleTrans detailVietnamese = new GoogleTrans();
            //Explain main of letter
            MessJson messExplaintion = new MessJson();
            //Pron
            MessJson messPron = new MessJson();
            //Vietnamese
            MessJson messVietnamese = new MessJson();
            //
            ChatfuelJson hello = new ChatfuelJson();
            AttachmentJson sound = new AttachmentJson();
            sound.attachment.type = "audio";
            Attachment2 attach = new Attachment2();
            if (contain.LastOrDefault() == '.')
            {
                messPron.text = "";
                hello.messages.Add(messPron);
                return JsonConvert.SerializeObject(hello);
            }
            Payload2 payload = new Payload2();
            //
            if (dictTemp != null)
            {
                messPron.text = dictTemp.Pron;
                messExplaintion.text = dictTemp.MeanEn;
                messVietnamese.text = dictTemp.MeanVi;
                sound.attachment.payload.url = dictTemp.SoundUrl;
            }
            else
            {
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
                        messPron.text = dict.m_Pron;
                        messExplaintion.text = dict.m_Explanation.First().m_UseCase;
                        
                    }
                    catch
                    {
                        isSaveCached = false;
                        messExplaintion.text = contain;
                    }
                    detailVietnamese = await _clientDictionaryService.startGoogleDetailTrans(contain);

                    try
                    {
                        messVietnamese.text = detailVietnamese.dict.First().pos + ": ";
                        foreach (var item in detailVietnamese.dict.First().terms)
                        {
                            messVietnamese.text += item + ", ";
                        }
                    }
                    catch
                    {
                        isSaveCached = false;
                        messVietnamese.text = detailVietnamese.sentences.FirstOrDefault().trans;
                    }

                }
                sound.attachment.payload.url = dict.m_SoundUrl;
                //Add database
                cacheDict = new DictCache();
                cacheDict.VocaID = contain;
                cacheDict.Pron = messPron.text;
                cacheDict.MeanEn = messExplaintion.text;
                cacheDict.MeanVi = messVietnamese.text;
                cacheDict.SoundUrl = dict.m_SoundUrl;
                _dictCache.Add(cacheDict);
                if(isSaveCached)
                    _dictCache.Save();
                dictTemp = cacheDict;
            }
            DetailOurWord detailWord = _detailOutWordService.findDetailOurWord(userid, dictTemp.Id);
            if (detailWord == null)
            {
                hello.Selected = false;
            }
            else
            {
                hello.Selected = true;
            }
            hello.messages.Add(messPron);
            hello.messages.Add(messVietnamese);
            hello.messages.Add(messExplaintion);
            hello.messages.Add(sound);
            try
            {
                hello.strVoca = ToTitleCase(contain);
                ApplicationUser currentUser = _service.listUserID().Where(x => x.Id_Messenger == id).FirstOrDefault();
                NotificationHub.sendNoti(currentUser.Email, JsonConvert.SerializeObject(hello));
            }
            catch
            {

            }
            return JsonConvert.SerializeObject(hello);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> notifyMessenger()
        {
            //  var listUser = _detailOutWordService.
            ListUserNofity listUserNotify = new ListUserNofity();
            List<ApplicationUser> listUser = _service.listUserID();
            List<int> listIDWord = new List<int>();

            foreach (var userDetail in listUser)
            {
                RemindUser reminderUser = new RemindUser();
                reminderUser.IdMess = userDetail.Id_Messenger;
                listIDWord = _detailOutWordService.listIdOutWord(userDetail.Id, DateTime.Now.Hour);
                //Update thoi gian 
                //
                try
                {
                    if (listIDWord != null || listIDWord.Count != 0)
                    {
                        await sendNotificationEnlishVoca(listIDWord, userDetail.Id_Messenger);
                    }
                }
                catch (Exception e)
                {

                }
            }

            return JsonConvert.SerializeObject(listUserNotify);
        }
        [AllowAnonymous]
        public void studyVoca()
        {
            List<int> randDomPost = randomPosition();
            List<TracNghiem> dataTracNghiem = new List<TracNghiem>();
            List<int> listIDWord = new List<int>();
            var currentUser = _service.GetUserById(User.Identity.GetUserId());
            listIDWord = _detailOutWordService.listIdOutWord(currentUser.Id, -1);
            foreach (var id in listIDWord)
            {
                dataTracNghiem.Add(RandomTracNghiemChoVoca(id));
            }
            //
        }
        protected TracNghiem RandomTracNghiemChoVoca(int idWord)
        {
            TracNghiem cauTracNghiem = new TracNghiem();
            var vocaCanHoc = _dictCache.GetById(idWord);
            Random rnd = new Random();
            var listDict = _dictCache.GetAll();
            int pos = rnd.Next(0, listDict.Count);
            var ramdomPo = randomPosition();
            cauTracNghiem.Question = vocaCanHoc.MeanEn;
            for (int i = 0; i < 4; i++)
            {
                DapAn dapAn = new DapAn();
                if (i == 0)
                {
                    dapAn.Checked = true;
                    dapAn.Contain = vocaCanHoc.VocaID;
                    cauTracNghiem.ABCD[ramdomPo[i]] = dapAn;
                }
                else
                {
                    int posDictCache = rnd.Next(0, listDict.Count);
                    dapAn.Contain = listDict.ElementAt(posDictCache).VocaID;
                    cauTracNghiem.ABCD[ramdomPo[i]] = dapAn;
                }

            }
            return cauTracNghiem;
        }
        protected List<int> randomPosition()
        {
            List<int> posResult = new List<int>();

            Random rnd = new Random();
            int count = 0;
            while (true)
            {
                int pos = rnd.Next(0, 4);
                if (posResult.Where(x => x == pos).ToList().Count == 0)
                {
                    posResult.Add(pos);
                    count++;
                    if (count > 3)
                    {
                        break;
                    }
                }
            }
            return posResult;
        }
        protected async Task<string> sendNotificationEnlish()
        {
            string paramChatfuel = "";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var responseVoca = notifyMessenger().Result;
            ListUserNofity listNofity = JsonConvert.DeserializeObject<ListUserNofity>(responseVoca);
            foreach (var userNoti in listNofity.reminduser)
            {
                paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + userNoti.IdMess + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a28393be4b0d0e6fdab76b3";
                for (int i = 1; i <= 5; i++)
                {
                    paramChatfuel += "&word" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].voca : "");
                    paramChatfuel += "&pron" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].pron : "");
                    paramChatfuel += "&meanvn" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].meanVN : "");
                    paramChatfuel += "&meanen" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].usecase : "");
                }
                paramChatfuel += ChatBotMessenger.getNotiNull();
                var response = await client.PostAsync(paramChatfuel, null);
            }
            return "";
        }
        protected async Task<string> sendNotificationEnlishVoca(List<int> listIdWord, string messID)
        {
            //ChatfuelJson jsonMessenger = new ChatfuelJson();
            //MessJson messExplaintion = new MessJson();
            //MessJson messPron = new MessJson();
            //MessJson messVietnamese = new MessJson();
            //AttachmentJson sound = new AttachmentJson();
            // jsonMessenger.recipient.id = messID;
            //
            bool isCheck = false;
            foreach (var idWord in listIdWord)
            {
                isCheck = true;
                JsonMessengerText jsonTextMessenger = new JsonMessengerText();
                MessJson messExplaintion = new MessJson();
                jsonTextMessenger.recipient.id = messID;
                JsonMessengerText jsonSoundMessenger = new JsonMessengerText();
                jsonSoundMessenger.recipient.id = messID;

                AttachmentJson sound = new AttachmentJson();
                var tempWord = _dictCache.GetById(idWord);
                var infoVoca = _dictCache.GetAll().Where(x => x.VocaID == tempWord.VocaID).FirstOrDefault();
                messExplaintion.text = infoVoca.VocaID;
                messExplaintion.text += "\r\n" + infoVoca.Pron;
                messExplaintion.text += "\r\n" + infoVoca.MeanEn;
                messExplaintion.text += "\r\n" + infoVoca.MeanVi;
                sound.attachment.payload.url = infoVoca.SoundUrl;
                //
                jsonTextMessenger.message = messExplaintion;
                jsonSoundMessenger.message = sound;
                //
                string temp = JsonConvert.SerializeObject(jsonTextMessenger);
                temp = JsonConvert.SerializeObject(jsonSoundMessenger);
                PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
                PostRaw("", JsonConvert.SerializeObject(jsonSoundMessenger));

            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (isCheck)
                await client.PostAsync("https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + messID + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a3fc60de4b037186c58ec99", null);
            return "";
        }
        //Post method
        [AllowAnonymous]
        private string PostRaw(string url, string data)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://graph.facebook.com/v2.6/me/messages?access_token=EAACn86pGAioBAAJ8gqV2eRJPN5Yznq3rXG9az1IpesyWJTem3HlchCQNEfSfxQmDxMtlvBpyclx2CvLDf9Im2ZCUPVgzty3IURuxNJ2STjUZBvTVGprkNs7NjnGKLMbuu0ZAwr99cFtcSxHTSfpblqkiLYtkKbWUZBZBBMGDSGZBYcpUxxo3rp");
                request.ContentType = "application/json";
                request.Method = "POST";
                using (var requestWriter = new StreamWriter(request.GetRequestStream()))
                {
                    requestWriter.Write(data);
                    requestWriter.Flush();
                    requestWriter.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                if (response == null)
                    throw new InvalidOperationException("GetResponse returns null");

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return "";
            }
        }
        public string verifyToken()
        {
            MessJson messPron = new MessJson();
            var userInfo = _service.GetUserById(User.Identity.GetUserId());
            messPron.text = userInfo.LastName + " " + userInfo.FirstName;
            return JsonConvert.SerializeObject(messPron);
        }
        
        public async Task<string> getDictToExtension(string contain)
        {
            bool isVN = false;
            string userid = User.Identity.GetUserId();
            contain = contain.ToString().ToLower().Trim();
            DictCache dictTemp = new DictCache() ;
            //Tim trong cache nếu ko có thì request crawler
            try
            {
                dictTemp = _dictCache.findWordCache(contain);
            }
            catch(Exception e)
            {

            }
            //post
            //
            bool isSaveCached = true;
            DictCache cacheDict;
            OxfordDict dict = new OxfordDict();
            GoogleTrans googleTransJson = new GoogleTrans();
            GoogleTrans detailVietnamese = new GoogleTrans();
            //Explain main of letter
            MessJson messExplaintion = new MessJson();
            //Pron
            MessJson messPron = new MessJson();
            //Vietnamese
            MessJson messVietnamese = new MessJson();
            //
            ChatfuelJson hello = new ChatfuelJson();
            AttachmentJson sound = new AttachmentJson();
            sound.attachment.type = "audio";
            Attachment2 attach = new Attachment2();
            if (contain.LastOrDefault() == '.')
            {
                messPron.text = "";
                hello.messages.Add(messPron);
                return JsonConvert.SerializeObject(hello);
            }
            Payload2 payload = new Payload2();
            //
            if (dictTemp != null)
            {
                messPron.text = dictTemp.Pron;
                messExplaintion.text = dictTemp.MeanEn;
                messVietnamese.text = dictTemp.MeanVi;
                sound.attachment.payload.url = dictTemp.SoundUrl;
            }
            else
            {
                int size = contain.Split(' ').Length;
                if (size > 1)
                {
                    isSaveCached = false;
                    googleTransJson = await _clientDictionaryService.startGoogleTrans(contain);
                    hello.strVietnamese = googleTransJson.src;
                    if (googleTransJson.src == "vi")
                        isVN = true;
                    messExplaintion.text = googleTransJson.sentences[0].trans;
                }
                else
                {

                    try
                    {
                        dict = await _clientDictionaryService.startCrawlerOxford(contain);
                        messPron.text = dict.m_Pron;
                        messExplaintion.text = dict.m_Explanation.First().m_UseCase;
                        
                    }
                    catch
                    {
                        isSaveCached = false;
                        messExplaintion.text = contain;
                    }
                    detailVietnamese = await _clientDictionaryService.startGoogleDetailTrans(contain);
                    hello.strVietnamese = detailVietnamese.src;
                    if (detailVietnamese.src == "vi")
                        isVN = true;
                    try
                    {

                        messVietnamese.text = detailVietnamese.dict.First().pos + ": ";
                        foreach (var item in detailVietnamese.dict.First().terms)
                        {
                            messVietnamese.text += item + ", ";
                        }
                    }
                    catch
                    {
                        isSaveCached = false;
                        messVietnamese.text = detailVietnamese.sentences.FirstOrDefault().trans;
                    }

                }
                sound.attachment.payload.url = dict.m_SoundUrl;
                //Add database
                cacheDict = new DictCache();
                cacheDict.VocaID = contain;
                cacheDict.Pron = messPron.text;
                cacheDict.MeanEn = messExplaintion.text;
                cacheDict.MeanVi = messVietnamese.text;
                cacheDict.SoundUrl = dict.m_SoundUrl;
                _dictCache.Add(cacheDict);
                if(isSaveCached)
                    _dictCache.Save();
                dictTemp = cacheDict;
            }
            DetailOurWord detailWord = _detailOutWordService.findDetailOurWord(userid, dictTemp.Id);
            if (detailWord == null)
            {
                hello.Selected = false;
            }
            else
            {
                hello.Selected = true;
            }
            hello.messages.Add(messPron);
            hello.messages.Add(messVietnamese);
            hello.messages.Add(messExplaintion);
            hello.messages.Add(sound);
            if (isVN)
                return null;
            return JsonConvert.SerializeObject(hello);
        }
        [AllowAnonymous]
        public async Task<string> googleTranslator(string contain)
        {
            GoogleTrans googleTransJson = new GoogleTrans();
            googleTransJson = await _clientDictionaryService.startGoogleTrans(contain);
            return googleTransJson.sentences[0].trans;
        }
        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        [AllowAnonymous]
        public async Task<string> searchDictViaBot(string contain, string id = "1554844547918927", string userid= null)
        {
            bool multiLetters = false;
            contain = contain.ToString().ToLower().Trim();
            //Tim trong cache nếu ko có thì request crawler
            var dictTemp = _dictCache.findWordCache(contain);
            //post
            //
            bool isSaveCached = true;
            DictCache cacheDict = new DictCache();
            OxfordDict dict = new OxfordDict();
            GoogleTrans googleTransJson = new GoogleTrans();
            GoogleTrans detailVietnamese = new GoogleTrans();
            //Explain main of letter
            MessJson messExplaintion = new MessJson();
            //Pron
            MessJson messPron = new MessJson();
            //Vietnamese
            MessJson messVietnamese = new MessJson();
            //
            ChatfuelJson hello = new ChatfuelJson();
            AttachmentJson sound = new AttachmentJson();
            sound.attachment.type = "audio";
            Attachment2 attach = new Attachment2();
            //Json is returned Customer
            AttachmentJson soundDic = new AttachmentJson();
            MessJson messExplaintionDict = new MessJson();
            MessJson resultGoogle = new MessJson();
            //
            JsonMessengerText jsonTextMessenger = new JsonMessengerText();
            jsonTextMessenger.recipient.id = id;
            JsonMessengerText jsonSoundMessenger = new JsonMessengerText();
            jsonSoundMessenger.recipient.id = id;
            messExplaintionDict.text += contain;

            Payload2 payload = new Payload2();
            //
            if (dictTemp != null)
            {
                messPron.text = dictTemp.Pron;
                messExplaintion.text = dictTemp.MeanEn;
                messVietnamese.text = dictTemp.MeanVi;
                sound.attachment.payload.url = dictTemp.SoundUrl;
                //
                messExplaintionDict.text += "\r\n" + dictTemp.Pron;
                messExplaintionDict.text += "\r\n" + dictTemp.MeanEn;
                messExplaintionDict.text += "\r\n" + dictTemp.MeanVi;
                soundDic.attachment.payload.url = dictTemp.SoundUrl;

            }
            else
            {
                int size = contain.Split(' ').Length;
                if (size > 1)
                {
                    multiLetters = true;
                    googleTransJson = await _clientDictionaryService.startGoogleTrans(contain);
                    foreach(var sentence in googleTransJson.sentences)
                    {
                        messExplaintion.text += sentence.trans;
                        resultGoogle.text +=  sentence.trans;
                    }
                    
                   // resultGoogle.text = googleTransJson.sentences[0].trans;
                    //messExplaintionDict.text +="\r\n" + googleTransJson.sentences[0].trans;
                }
                else
                {

                    try
                    {
                        dict = await _clientDictionaryService.startCrawlerOxford(contain);
                        messPron.text = dict.m_Pron;
                        messExplaintion.text = dict.m_Explanation.First().m_UseCase;
                        //
                        messExplaintionDict.text += "\r\n" + dict.m_Pron;
                        messExplaintionDict.text += "\r\n" + dict.m_Explanation.First().m_UseCase;

                    }
                    catch
                    {
                        isSaveCached = false;
                        messExplaintion.text = contain;
                        messExplaintionDict.text += "\r\n" + contain;
                    }
                    try
                    {
                        detailVietnamese = await _clientDictionaryService.startGoogleDetailTrans(contain);
                    }
                    catch
                    {
                        detailVietnamese = null;
                    }
                    try
                    {
                        messVietnamese.text = detailVietnamese.dict.First().pos + ": ";
                        foreach (var item in detailVietnamese.dict.First().terms)
                        {
                            messVietnamese.text += item + ", ";
                        }
                        messExplaintionDict.text += "\r\n" + messVietnamese.text;
                    }
                    catch
                    {
                        isSaveCached = false;
                        messVietnamese.text = detailVietnamese.sentences.FirstOrDefault().trans;
                        messExplaintionDict.text += "\r\n"+ detailVietnamese.sentences.FirstOrDefault().trans;
                    }

                }
                sound.attachment.payload.url = dict.m_SoundUrl;
                soundDic.attachment.payload.url = dict.m_SoundUrl;
                //Add database
                cacheDict = new DictCache();
                cacheDict.VocaID = contain;
                cacheDict.Pron = messPron.text;
                cacheDict.MeanEn = messExplaintion.text;
                cacheDict.MeanVi = messVietnamese.text;
                cacheDict.SoundUrl = dict.m_SoundUrl;
                _dictCache.Add(cacheDict);
                if (isSaveCached)
                    _dictCache.Save();
                dictTemp = cacheDict;
            }
            //DetailOurWord detailWord = _detailOutWordService.findDetailOurWord(userid, dictTemp.Id);
            //if (detailWord == null)
            //{
            //    hello.Selected = false;
            //}
            //else
            //{
            //    hello.Selected = true;
            //}
            hello.messages.Add(messPron);
            hello.messages.Add(messVietnamese);
            hello.messages.Add(messExplaintion);
            hello.messages.Add(sound);
            try
            {
                hello.strVoca = ToTitleCase(contain);
                ApplicationUser currentUser = _service.listUserID().Where(x => x.Id_Messenger == id).FirstOrDefault();
                NotificationHub.sendNoti(currentUser.Email, JsonConvert.SerializeObject(hello));
            }
            catch
            {

            }

            if(multiLetters)
            {
                jsonTextMessenger.message = messExplaintionDict;
                jsonSoundMessenger.message = resultGoogle;
                PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
                PostRaw("", JsonConvert.SerializeObject(jsonSoundMessenger));
                return "";
            }

            jsonTextMessenger.message = messExplaintionDict;
            jsonSoundMessenger.message = soundDic;
            PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
            PostRaw("", JsonConvert.SerializeObject(jsonSoundMessenger));
            return JsonConvert.SerializeObject(hello);
        }

    }
}
