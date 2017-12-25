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
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Framework.Model.Google;
using Newtonsoft.Json;
using System.Net;
using LiteDB;

namespace Framework.Controllers
{
    public class Item
    {
        public int Id { get; set; }
        public string Id_Messenger { get; set; }
        public int Status { get; set; }
    }
    public class LearningController : LayoutController
    {
       // public static int isContinue = 0;
        //public static int m_countQues = 0;
        private const string DB_PATH = @"~/App_Data/OneData.db";
        IClientLearningService _clientLearningService;
        IOurWordService _ourWordService;
        IDetailOurWordService _detailOutWordService;
        IDictCacheService _dictCache;
        IToiecGroupService _toiecService;
        public LearningController(ILayoutService layoutService,
            IClientLearningService clientLearningService,
                    IDetailOurWordService detailOutWordService,
            IOurWordService ourWordService,
            IDictCacheService dictCache,
            IToiecGroupService toiecService
            )
            : base(layoutService)
        {
            _clientLearningService = clientLearningService;
            _detailOutWordService = detailOutWordService;
            _ourWordService = ourWordService;
            _dictCache = dictCache;
            _toiecService = toiecService;
        }

        MultipleChoiceViewModel MultipleChoiceViewModel
        {
            get
            {
                return (MultipleChoiceViewModel)_viewModel;
            }
        }

        WordFormViewModel WordFormViewModel
        {
            get
            {
                return (WordFormViewModel)_viewModel;
            }
        }

        ListeningViewModel ListeningViewModel
        {
            get
            {
                return (ListeningViewModel)_viewModel;
            }
        }

        ReadingViewModel ReadingViewModel
        {
            get
            {
                return (ReadingViewModel)_viewModel;
            }
        }

        GrammarViewModel GrammarViewModel
        {
            get
            {
                return (GrammarViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option)
        {
            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "wordform":
                        {
                            _viewModel = new WordFormViewModel();
                            CreateLayoutView("Từ vựng");
                            break;
                        }
                    case "listening":
                        {
                            _viewModel = new ListeningViewModel();
                            CreateLayoutView("Tập nghe");
                            break;
                        }
                    case "reading":
                        {
                            _viewModel = new ReadingViewModel();
                            CreateLayoutView("Tập đọc");
                            break;
                        }
                    case "grammar":
                        {
                            _viewModel = new GrammarViewModel();
                            CreateLayoutView("Ngữ pháp");
                            break;
                        }
                    case "multiplechoice":
                        {
                            _viewModel = new MultipleChoiceViewModel();
                            CreateLayoutView("Trắc nghiệm");
                            break;
                        }
                    default:
                        {
                            _viewModel = new LearningViewModel();
                            CreateLayoutView("Tự học");
                            break;
                        }
                }
            }
            else
            {
                _viewModel = new LearningViewModel();
                CreateLayoutView("Tự học");
                ViewBag.page = "Index";
                return View(_viewModel);
            }

            return View("Index", "~/Views/Layout/_LearningLayout.cshtml", _viewModel);
        }

        [HttpGet]
        public ActionResult WordForm()
        {
            _viewModel = new WordFormViewModel();
            return PartialView("_WordForm", WordFormViewModel);
        }

        [HttpGet]
        public ActionResult Listening()
        {
            _viewModel = new ListeningViewModel();
            return PartialView("_Listening", ListeningViewModel);
        }

        [HttpGet]
        public ActionResult Reading()
        {
            _viewModel = new ReadingViewModel();
            return PartialView("_Reading", ReadingViewModel);
        }

        public ActionResult Grammar()
        {
            _viewModel = new GrammarViewModel();
            return PartialView("_Grammar", GrammarViewModel);
        }

        protected TracNghiem RandomTracNghiemChoVoca(int idWord)
        {
            TracNghiem cauTracNghiem = new TracNghiem();
            var vocaCanHoc = _ourWordService.GetById(idWord);
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
                    dapAn.Contain = vocaCanHoc.Word;
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

        [AllowAnonymous]
        public ActionResult MultipleChoice()
        {
            _viewModel = new MultipleChoiceViewModel();
            List<int> randDomPost = randomPosition();
            List<TracNghiem> dataTracNghiem = new List<TracNghiem>();

            List<int> listIDWord = new List<int>();
            var currentUser = _service.GetUserById(User.Identity.GetUserId());
            listIDWord = _detailOutWordService.listIdOutWord(currentUser.Id, -1);
            foreach (var id in listIDWord)
            {
                dataTracNghiem.Add(RandomTracNghiemChoVoca(id));
            }
            MultipleChoiceViewModel.listTracNghiem = dataTracNghiem;
            return PartialView("_MultipleChoice", MultipleChoiceViewModel);
        }
        //MultiChoice Toiec
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> MultipleChoiceToiec()
        {
            _viewModel = new MultipleChoiceViewModel();
            List<int> randDomPost = randomPosition();
            List<TracNghiem> dataTracNghiem = new List<TracNghiem>();
            var listToiecQues =  _toiecService.GetToiecExamList();
            foreach(var item in listToiecQues.data)
            {
                TracNghiem tracnghiem = new TracNghiem();
                tracnghiem.Question = item.message;
                tracnghiem.UrlImage = item.imageURL;
                //Tạo random postion
                var ramdomPo = randomPosition();
                DapAn dapAnTracNghiem = new DapAn();
                if (item.DapAn == "a" || item.DapAn == "A")
                {
                    dapAnTracNghiem.Checked = true;
                    tracnghiem.ABCD[0] = dapAnTracNghiem;
                }                
                else if (item.DapAn == "b" || item.DapAn == "B")
                {
                    dapAnTracNghiem.Checked = true;
                    tracnghiem.ABCD[1] = dapAnTracNghiem;
                }
                else if (item.DapAn == "c" || item.DapAn == "C")
                {
                    dapAnTracNghiem.Checked = true;
                    tracnghiem.ABCD[2] = dapAnTracNghiem;
                }
                else if (item.DapAn == "d" || item.DapAn == "D")
                {
                    dapAnTracNghiem.Checked = true;
                    tracnghiem.ABCD[3] = dapAnTracNghiem;
                }

                
                dataTracNghiem.Add(tracnghiem);
            }
            MultipleChoiceViewModel.listTracNghiem = dataTracNghiem;
            return PartialView("_MultipleChoice", MultipleChoiceViewModel);
        }
        //
        [AllowAnonymous]
        public async Task autoGeneratorToiecExam()
        {
            var listToiecQues = await _toiecService.GetListFeedTextOfGroup();
        }
        [AllowAnonymous]
        public async Task<string> multiplechoiceOnline(string id)
        {
            try
            {
                int numberTrue = 0;
                int numberFalse = 0;
                MessJson messExplaintion = new MessJson();
                messExplaintion.text = "";
                //create
                using (var db = new LiteDatabase(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OneData.db")))
                {
                    var items = db.GetCollection<Item>("items");
                    var item = items.FindOne(i => i.Id_Messenger == id);
                    //Create new item
                    if (item == null)
                    {
                        Item newItem = new Item();
                        newItem.Id_Messenger = id;
                        newItem.Status = 0;
                        items.Insert(newItem);
                    }
                }
                //
                List<int> randDomPost = randomPosition();
                List<TracNghiem> dataTracNghiem = new List<TracNghiem>();

                List<int> listIDWord = new List<int>();
                var currentUser = _service.listUserID().Where(x => x.Id_Messenger == id.ToString()).FirstOrDefault();
                listIDWord = _detailOutWordService.listIdOutWord(currentUser.Id, -1);
                foreach (var idWord in listIDWord)
                {
                    dataTracNghiem.Add(RandomTracNghiemChoVoca(idWord));
                }

                int m_countQues = 0;
                bool isBreak = false;
                //Gửi đi từng question
                while (true)
                {
                    UpdateLiteDb(id, 0);
                    JsonMessengerText jsonMessenger = new JsonMessengerText();

                    jsonMessenger.recipient.id = id;

                    //tao 1 cau trac nghiem gui cho messenger
                    MessageQuick messQuick = new MessageQuick();
                    try
                    {
                        messQuick.text = dataTracNghiem[m_countQues].Question;

                    }
                    catch
                    {
                        isBreak = true;
                        break;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        QuickReplyMess replay = new QuickReplyMess();
                        replay.payload = dataTracNghiem[m_countQues].ABCD[i].Checked.ToString();
                        replay.title = dataTracNghiem[m_countQues].ABCD[i].Contain + ".";
                        messQuick.quick_replies.Add(replay);
                    }
                    jsonMessenger.message = messQuick;
                    //send di
                    var temp = JsonConvert.SerializeObject(jsonMessenger);
                    PostRaw("", JsonConvert.SerializeObject(jsonMessenger));
                    int countTime = 0;
                    int isContinue = 0;
                    while (isContinue == 0)
                    {
                        Task.WaitAll(Task.Delay(1000));
                        using (var db = new LiteDatabase(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OneData.db")))
                        {
                            // Get a collection (or create, if not exits)
                            var col = db.GetCollection<Item>("items");
                            isContinue = col.FindOne(x => x.Id_Messenger == id).Status;
                        }
                        //Dap an sai
                        if (isContinue == 1)
                        {
                            //m_countQues++;
                            numberFalse++;
                            // isContinue = 0;
                            break;
                        }
                        //Dap an dung
                        else if (isContinue == 2)
                        {
                            m_countQues++;
                            numberTrue++;
                            if (m_countQues > dataTracNghiem.Count())
                                isBreak = true;
                            break;
                        }
                        //het gio
                        if (countTime > 10)
                        {
                            //Update status
                            UpdateLiteDb(id, 0);
                            //
                            m_countQues = dataTracNghiem.Count;
                            countTime = 0;
                            break;
                            //  m_countQues--;
                        }
                        countTime++;
                    }
                    if (isBreak)
                        break;
                }
                JsonMessengerText jsonTextMessenger = new JsonMessengerText();
                jsonTextMessenger.recipient.id = id;
                messExplaintion.text = "Hết giờ ! chúc bạn học tốt "+ "\r\n"+"Số câu đúng : "+numberTrue.ToString() + "\r\n" + "Số câu sai : " + numberFalse.ToString();
                jsonTextMessenger.message = messExplaintion;
                PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
                //
                return "";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
        [AllowAnonymous]
        public ActionResult ReceivePost(BotRequest data)
        {
            try
            {
                string strResult = data.entry.FirstOrDefault().messaging.FirstOrDefault().message.quick_reply.payload;
                string idMess = data.entry.FirstOrDefault().messaging.FirstOrDefault().sender.id;
                if (strResult == "True")
                {
                    try
                    {
                        JsonMessengerText jsonTextMessenger = new JsonMessengerText();
                        jsonTextMessenger.recipient.id = idMess;
                        MessJson messExplaintion = new MessJson();
                        messExplaintion.text = "Chính xác";
                        jsonTextMessenger.message = messExplaintion;
                        PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
                        //isContinue = 2;
                        //Update status
                        UpdateLiteDb(jsonTextMessenger.recipient.id, 2);
                    }
                    catch
                    {

                    }
                }
                else if(strResult == "False")
                {
                    try
                    {
                        JsonMessengerText jsonTextMessenger = new JsonMessengerText();
                        jsonTextMessenger.recipient.id = idMess;
                        MessJson messExplaintion = new MessJson();
                        messExplaintion.text = "Sai rồi nha bạn vui lòng làm lại";
                        jsonTextMessenger.message = messExplaintion;
                        PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
                        // isContinue = 1;
                        UpdateLiteDb(jsonTextMessenger.recipient.id, 1);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                //try
                //{
                //    if (data.entry.FirstOrDefault().messaging.FirstOrDefault().message == null)
                //    {
                //        JsonMessengerText jsonTextMessenger = new JsonMessengerText();
                //        jsonTextMessenger.recipient.id = data.entry.FirstOrDefault().messaging.FirstOrDefault().sender.id;
                //        MessJson messExplaintion = new MessJson();
                //        messExplaintion.text = "Có lỗi xay ra, mong bạn thông cảm !";
                //        jsonTextMessenger.message = messExplaintion;
                //        PostRaw("", JsonConvert.SerializeObject(jsonTextMessenger));
                //        isContinue = 1;
                //    }
                //}
                //catch
                //{

                //}
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
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
        public void UpdateLiteDb(string id, int status)
        {
            using (var db = new LiteDatabase(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OneData.db")))
            {
                var items = db.GetCollection<Item>("items");
                var item = items.FindOne(i => i.Id_Messenger == id);
                //Create new item
                if (item == null)
                {
                    Item newItem = new Item();
                    newItem.Id_Messenger = id;
                    newItem.Status = status;
                    items.Insert(newItem);
                }
                else
                {
                    item.Status = status;
                    items.Update(item);
                }
            }
        }
    }
}
