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
using Microsoft.AspNet.Identity;
using Framework.Model;
using Framework.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net;
using Framework.Model.Bot;
using LiteDB;

namespace Framework.Controllers
{
    public class ItemMessenger
    {
        public ItemMessenger()
        {
            IsFile = false;
            IsPayload = false;
        }
        public int Id { get; set; }
        public string Id_Messenger { get; set; }
        public int Status { get; set; }
        public string Previous_Msg { set; get; }
        public string PayLoad { set; get; }
        public bool IsFile { set; get; }
        public bool IsPayload { set; get; }
    }

    public class YourAccountController : LayoutController
    {
        string pageToken = "EAACn86pGAioBAAJ8gqV2eRJPN5Yznq3rXG9az1IpesyWJTem3HlchCQNEfSfxQmDxMtlvBpyclx2CvLDf9Im2ZCUPVgzty3IURuxNJ2STjUZBvTVGprkNs7NjnGKLMbuu0ZAwr99cFtcSxHTSfpblqkiLYtkKbWUZBZBBMGDSGZBYcpUxxo3rp";
        string appSecret = "e4bb9e8c052fd8008d6d3b3d1ac7c9b9";
        IApplicationUserService _applicationUserService;
        IClientFriendService _friendService;
        IClientNotificationService _notificationService;

        public YourAccountController(ILayoutService layoutService,
            IApplicationUserService applicationUserService,
             IClientFriendService friendService,
            IClientNotificationService notificationService)
            : base(layoutService)
        {
            _applicationUserService = applicationUserService;
            UserManager = new UserManager<ApplicationUser>(new ApplicationUserStore(new ApplicationDbContext()));
            _friendService = friendService;
            _notificationService = notificationService;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        UpdateInfoViewModel UpdateInfoViewModel
        {
            get
            {
                return (UpdateInfoViewModel)_viewModel;
            }
        }

        UpdatePasswordViewModel UpdatePasswordViewModel
        {
            get
            {
                return (UpdatePasswordViewModel)_viewModel;
            }
        }

        UpdateSettingViewModel UpdateSettingViewModel
        {
            get
            {
                return (UpdateSettingViewModel)_viewModel;
            }
        }
        NotifyViewModel NotifyViewModel
        {
            get
            {
                return (NotifyViewModel)_viewModel;
            }
        }

        RequestsViewModel RequestsViewModel
        {
            get
            {
                return (RequestsViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option, string userType)
        {
            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "updateinfo":
                        {
                            _viewModel = new UpdateInfoViewModel();
                            CreateLayoutView("Cập nhật thông tin");
                            break;
                        }
                    case "updatepassword":
                        {
                            _viewModel = new UpdatePasswordViewModel();
                            CreateLayoutView("Cập nhật mật khẩu");
                            break;
                        }
                    case "updatesetting":
                        {
                            _viewModel = new UpdateSettingViewModel();
                            CreateLayoutView("Cài đặt");
                            break;
                        }
                    case "notification":
                        {
                            _viewModel = new NotificationViewModel();
                            CreateLayoutView("Thông báo");
                            break;
                        }
                    case "requests":
                        {
                            _viewModel = new RequestsViewModel();
                            CreateLayoutView("Lời mời kết bạn");
                            break;
                        }
                    default:
                        {
                            _viewModel = new UpdateInfoViewModel();
                            CreateLayoutView("Cập nhật thông tin");
                            break;
                        }
                }
            }
            else
            {
                _viewModel = new UpdateInfoViewModel();
                CreateLayoutView("Cập nhật thông tin");
            }
            ViewBag.newMember = userType;
            ViewBag.listRequest = _friendService.GetRelationship(User.Identity.GetUserId()).Count - 1;
            ViewBag.listNotification = _notificationService.getAllNotification(User.Identity.GetUserId()).Count - 1;

            return View(_viewModel);
        }

        [HttpGet]
        public ActionResult UpdateInfo()
        {
            _viewModel = new UpdateInfoViewModel();
            CreateLayoutView("Cập nhật thông tin");
            FieldHelper.CopyNotNullValue(UpdateInfoViewModel, _viewModel.User);
            ViewBag.listRequest = _friendService.GetRelationship(User.Identity.GetUserId()).Count - 1;
            ViewBag.listNotification = _notificationService.getAllNotification(User.Identity.GetUserId()).Count - 1;

            return PartialView("_UpdateInfo", UpdateInfoViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateInfo(UpdateInfoViewModel model, string userType)
        {
            if (ModelState.IsValid)
            {
                var user = _service.GetUserById(User.Identity.GetUserId());
                FieldHelper.CopyNotNullValue(user, model);

                _applicationUserService.Update(user);
                try
                {
                    _applicationUserService.Save();
                    ViewBag.newMember = userType;
                    ViewBag.listRequest = _friendService.GetRelationship(User.Identity.GetUserId()).Count - 1;
                    ViewBag.listNotification = _notificationService.getAllNotification(User.Identity.GetUserId()).Count - 1;

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
            else
            {
                return Json(new
                {
                    result = "failed",
                    data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
                });
            }
        }

        [HttpGet]
        public ActionResult UpdatePassword()
        {
            _viewModel = new UpdatePasswordViewModel();
            CreateLayoutView("Quản lý tài khoản");
            ViewBag.listRequest = _friendService.GetRelationship(User.Identity.GetUserId()).Count - 1;
            ViewBag.listNotification = _notificationService.getAllNotification(User.Identity.GetUserId()).Count - 1;

            return PartialView("_UpdatePassword", UpdatePasswordViewModel);
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (HasPassword())
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return Json(new
                        {
                            result = "success"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            result = "failed",
                        });
                    }
                }
            }
            return Json(new
            {
                result = "failed",
                data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
            });
        }

        [HttpGet]
        public ActionResult UpdateSetting()
        {
            _viewModel = new UpdateSettingViewModel();
            ViewBag.listRequest = _friendService.GetRelationship(User.Identity.GetUserId()).Count - 1;
            ViewBag.listNotification = _notificationService.getAllNotification(User.Identity.GetUserId()).Count - 1;

            return PartialView("_UpdateSetting");
        }

        [HttpGet]
        public ActionResult Notification()
        {
            _viewModel = new NotifyViewModel();
            ViewBag.listRequest = _friendService.GetRelationship(User.Identity.GetUserId()).Count;
            CreateLayoutView("Thông báo");
            List<Notification> listNotification = _notificationService.getAllNotification(User.Identity.GetUserId());
            foreach (Notification notification in listNotification)
            {
                NotificationViewModel notificationViewModel = new NotificationViewModel();
                ApplicationUser userT = new ApplicationUser();
                userT = _service.GetUserById(notification.Id_User);
                FieldHelper.CopyNotNullValue(notificationViewModel, userT);
                FieldHelper.CopyNotNullValue(notificationViewModel, notification);
                NotifyViewModel.ListNotification.Add(notificationViewModel);
            }
            NotifyViewModel.ListNotification.OrderBy(x => x.CreatedDate);
            ViewBag.listNotification = listNotification.Count - 1;
            return PartialView("_Notification", NotifyViewModel);
        }

        [HttpGet]
        public ActionResult Requests()
        {
            bool flag = false;
            _viewModel = new RequestsViewModel();
            CreateLayoutView("Lời mời kết bạn");
            List<Friend> listRequest = _friendService.GetRelationship(User.Identity.GetUserId());
            foreach (Friend friend in listRequest)
            {
                NotiFriendViewModel notiFriendViewModel = new NotiFriendViewModel();
                ApplicationUser userT = new ApplicationUser();
                if (friend != null)
                {
                    switch (flag)
                    {
                        case false:
                            {
                                userT = _service.GetUserById(friend.Id_User);
                                notiFriendViewModel.Flag = false;
                                break;
                            }
                        case true:
                            {
                                userT = _service.GetUserById(friend.Id_Friend);
                                notiFriendViewModel.Flag = true;
                                break;
                            }
                    }
                    FieldHelper.CopyNotNullValue(notiFriendViewModel, userT);
                    FieldHelper.CopyNotNullValue(notiFriendViewModel, friend);
                    RequestsViewModel.ListRequest.Add(notiFriendViewModel);
                }
                else
                {
                    flag = true;
                }
            }
            RequestsViewModel.ListRequest.OrderBy(x => x.CreatedDate);
            ViewBag.listRequest = listRequest.Count - 1;
            ViewBag.listNotification = _notificationService.getAllNotification(User.Identity.GetUserId()).Count;
            return PartialView("_Requests", RequestsViewModel);
        }
        [AllowAnonymous]
        public ActionResult Get()
        {
            var querystrings = Request.QueryString;
            if (querystrings["hub.verify_token"] == pageToken)
            {
                var retVal = querystrings["hub.challenge"];
                return Json(int.Parse(retVal), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [AllowAnonymous]
        public async Task<ActionResult> ReceivePost(BotRequest data)
        {
            try
            {
                //If fast replay
                if (data.entry[0].messaging[0].message.quick_reply != null)
                {
                    var ctrlr = DependencyResolver.Current.GetService<LearningController>();
                    ctrlr.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlr);
                    ctrlr.ReceivePost(data);
                    //Update again
                    ItemMessenger previousMess = new ItemMessenger();
                    previousMess.Id_Messenger = data.entry[0].messaging[0].sender.id;
                    UpdatePreviousMsg(previousMess);
                    return null;
                }
            }
            catch
            {

            }
            //
            ItemMessenger previousMsg = getPreviousMsg(data.entry[0].messaging[0].sender.id);
            if(previousMsg!=null)
            {
                //Tn trước đó có chứa payload => đang trời cho 1 câu hỏi gì đó
                if (previousMsg.IsPayload == true)
                {
                    //Menu payload xac thuc
                    if (previousMsg.PayLoad == "XACTHUC")
                    {
                        var ctrlrDict = DependencyResolver.Current.GetService<AccountController>();
                        ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                        ctrlrDict.registerChatBot(data.entry[0].messaging[0].message.text, data.entry[0].messaging[0].sender.id);

                    }
                    else if (previousMsg.PayLoad == "LUYENTAP")
                    {

                    }
                    else if (previousMsg.PayLoad.IndexOf(@"POST_TYPE_") != -1)
                    {
                        var postType = int.Parse(previousMsg.PayLoad.Substring(10));
                        ChatBotMessenger.sendTextMeg(data.entry[0].messaging[0].sender.id, "🎉 Câu hỏi bạn đã được gửi đi 🎉 ");
                        var ctrlrDict = DependencyResolver.Current.GetService<PostController>();
                        ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                        await ctrlrDict.createNewPostViaFB(data.entry[0].messaging[0].sender.id, data.entry[0].messaging[0].message.text, postType);
                    }
                    else if (previousMsg.PayLoad.IndexOf(@"REPLAY_") != -1)
                    {
                        ChatBotMessenger.sendTextMeg(data.entry[0].messaging[0].sender.id, "🎉 Thank you 🎉 ");
                        //
                        var postID = int.Parse(previousMsg.PayLoad.Substring(7));
                        //
                        ApplicationUser currentUser = _service.GetUserByMessID(data.entry[0].messaging[0].sender.id);
                        //
                        CommentViewModel dataCmt = new CommentViewModel();
                        dataCmt.Id_User = currentUser.Id;
                        dataCmt.Id_Comment = 0;
                        dataCmt.Id_Post = postID;
                        dataCmt.Name = data.entry[0].messaging[0].message.text;
                        dataCmt.Content = data.entry[0].messaging[0].message.text;
                        var ctrlrDict = DependencyResolver.Current.GetService<PostController>();
                        ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                        ctrlrDict.Comment(dataCmt,true);
                    }
                    //Update again
                    ItemMessenger previousMess = new ItemMessenger();
                    previousMess.Id_Messenger = data.entry[0].messaging[0].sender.id;
                    UpdatePreviousMsg(previousMess);
                    return null;
                }
            }
            string imageToText = "";
            try
            {
                if(data.entry[0].messaging[0].postback != null && data.entry[0].messaging[0].message == null)
                {
                    ItemMessenger previousMess = new ItemMessenger();
                    previousMess.PayLoad = data.entry[0].messaging[0].postback.payload;
                    previousMess.Id_Messenger = data.entry[0].messaging[0].sender.id;
                    previousMess.Previous_Msg = data.entry[0].messaging[0].postback.title;
                    previousMess.Status = 1;
                    previousMess.IsPayload = true;
                    UpdatePreviousMsg(previousMess);
                    //xac thu token
                    if (data.entry[0].messaging[0].postback.payload.ToString() == "XACTHUC")
                    {
                        ChatBotMessenger.sendTextMeg(data.entry[0].messaging[0].sender.id, "🔑 Vui lòng nhập mã token sau tin nhắn này 🔑");
                        //var ctrlrDict = DependencyResolver.Current.GetService<AccountController>();
                        //ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                       // await ctrlrDict.registerChatBot(data.entry[0].messaging[0].message.text, data.entry[0].messaging[0].sender.id, "");
                    }
                    else if (data.entry[0].messaging[0].postback.payload.ToString() == "LUYENTAP")
                    {
                        var ctrlrDict = DependencyResolver.Current.GetService<LearningController>();
                        ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                        await ctrlrDict.multiplechoiceOnline(data.entry[0].messaging[0].sender.id);
                    }
                    else if (data.entry[0].messaging[0].postback.payload.ToString().IndexOf(@"POST_TYPE_") != -1)
                    {
                        ChatBotMessenger.sendTextMeg(data.entry[0].messaging[0].sender.id, "❓ Mời bạn nhập câu hỏi ❓");
                    }
                    else if (data.entry[0].messaging[0].postback.payload.ToString().IndexOf(@"REPLAY_") != -1)
                    {
                        ChatBotMessenger.sendTextMeg(data.entry[0].messaging[0].sender.id, "✍️ Mời bạn nhập câu trả lời ✍️");
                    }

                    return null;
                }

                //Tra tu dien
                if(data.entry[0].messaging[0].message.attachments != null &&  data.entry[0].messaging[0].postback == null)
                {
                    ItemMessenger previousMess = new ItemMessenger();
                    previousMess.Id_Messenger = data.entry[0].messaging[0].sender.id;
                    UpdatePreviousMsg(previousMess);
                    //
                    imageToText = data.entry[0].messaging[0].message.attachments[0].payload.url;
                    string textResult = await ConvertImageURLToBase64(data.entry[0].messaging[0].message.attachments[0].payload.url);
                    var ctrlrDict = DependencyResolver.Current.GetService<DictionaryController>();
                    ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                    await ctrlrDict.searchDictViaBot(textResult, data.entry[0].messaging[0].sender.id, "");
                    return null;
                }
                if (data.entry[0].messaging[0].message.text != "" && data.entry[0].messaging[0].message.quick_reply == null && data.entry[0].messaging[0].postback == null)
                {
                    ItemMessenger previousMess = new ItemMessenger();
                    previousMess.Id_Messenger = data.entry[0].messaging[0].sender.id;
                    UpdatePreviousMsg(previousMess);
                    //
                    var ctrlrDict = DependencyResolver.Current.GetService<DictionaryController>();
                    ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                    await ctrlrDict.searchDictViaBot(data.entry[0].messaging[0].message.text, data.entry[0].messaging[0].sender.id, "");
                    return null;
                }
            }
            catch
            {

            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [AllowAnonymous]
        private string PostRaw(string url, string data)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
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

        [HttpPost]
        public PartialViewResult NotiRequestSectionAccount(string username)
        {
            NotiRequestViewModel notiRequest = new NotiRequestViewModel();
            if (username != null)
            {
                bool flag = false;
                List<Friend> listRequest = _service.GetRelationship(_service.GetUserByUserName(username).Id);
                foreach (Friend friend in listRequest)
                {
                    NotiFriendViewModel notiFriendViewModel = new NotiFriendViewModel();
                    ApplicationUser userT = new ApplicationUser();
                    if (friend != null)
                    {
                        switch (flag)
                        {
                            case false:
                                {
                                    userT = _service.GetUserById(friend.Id_User);
                                    notiFriendViewModel.Flag = false;
                                    break;
                                }
                            case true:
                                {
                                    userT = _service.GetUserById(friend.Id_Friend);
                                    notiFriendViewModel.Flag = true;
                                    break;
                                }
                        }
                        FieldHelper.CopyNotNullValue(notiFriendViewModel, userT);
                        FieldHelper.CopyNotNullValue(notiFriendViewModel, friend);
                        notiRequest.ListRequest.Add(notiFriendViewModel);
                    }
                    else
                    {
                        flag = true;
                    }
                }
                notiRequest.ListRequest.OrderBy(x => x.CreatedDate);
            }
            return PartialView("_NotiFriend", notiRequest);
        }

        [HttpPost]
        public PartialViewResult NotificationSectionAccount(string username)
        {
            NotifyViewModel notify = new NotifyViewModel();
            if (username != null)
            {
                List<Notification> listNotification = _notificationService.getAllNotification(_service.GetUserByUserName(username).Id);
                foreach (Notification notification in listNotification)
                {
                    NotificationViewModel notificationViewModel = new NotificationViewModel();
                    ApplicationUser userT = new ApplicationUser();
                    userT = _service.GetUserById(notification.Id_User);
                    FieldHelper.CopyNotNullValue(notificationViewModel, userT);
                    FieldHelper.CopyNotNullValue(notificationViewModel, notification);
                    notify.ListNotification.Add(notificationViewModel);
                }
                notify.ListNotification.OrderBy(x => x.CreatedDate);
            }
            return PartialView("_Notify", notify);
        }
        //Demo image to text
        [AllowAnonymous]
        public async Task<string> ConvertImageURLToBase64(String url)
        {
            byte[] data;
            using (var webClient = new WebClient())
                data = webClient.DownloadData(url);
            string enc = Convert.ToBase64String(data);
            return await UploadDocument(enc);
        }
        public async Task<string> UploadDocument(string base64Image)
        {
            string result = "";
            try
            {
                List<RequestVision> listTempVision = new List<RequestVision>();
                RequestVision req = new RequestVision();
                req.image.content = base64Image;
                List<Feature> listFeature = new List<Feature>();
                Feature fea = new Feature();
                fea.type = "DOCUMENT_TEXT_DETECTION";
                listFeature.Add(fea);
                req.features = listFeature;
                listTempVision.Add(req);
                GoogleVisionJson json = new GoogleVisionJson();
                json.requests = listTempVision;
                string test = JsonConvert.SerializeObject(json);
                // ...
                using (var handler = new HttpClientHandler() { UseDefaultCredentials = true })
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    // Image = 123 (or any random value) Works.
                    var responseGoogleVision = await client.PostAsync("https://vision.googleapis.com/v1/images:annotate?key=AIzaSyBrlBUB3kMBFTB73hcIAeQmLu1xT1sLNGA", new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json"));
                    var responseContent = responseGoogleVision.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<GoogleVisionReponse>(responseContent).responses[0].fullTextAnnotation.text;

                }
                return result;
            }
            catch
            {
                return result;
            }
        }
        public void UpdatePreviousMsg(ItemMessenger inputItem)
        {
            using (var db = new LiteDatabase(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OneData.db")))
            {
                var items = db.GetCollection<ItemMessenger>("ItemMessengers");
                var item = items.FindOne(i => i.Id_Messenger == inputItem.Id_Messenger);
                //Create new item
                if (item == null)
                {

                    items.Insert(inputItem);
                }
                else
                {
                    item.Id_Messenger = inputItem.Id_Messenger;
                    item.IsFile = inputItem.IsFile;
                    item.IsPayload = inputItem.IsPayload;
                    item.PayLoad = inputItem.PayLoad;
                    item.Previous_Msg = inputItem.Previous_Msg;
                    item.Status = inputItem.Status;
                    items.Update(item);
                }
            }
        }
        public ItemMessenger getPreviousMsg(string IdMess)
        {
            using (var db = new LiteDatabase(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OneData.db")))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection<ItemMessenger>("ItemMessengers");
                return col.FindOne(x => x.Id_Messenger == IdMess);
            }
        }
    }
}
