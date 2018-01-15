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

namespace Framework.Controllers
{

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
            string imageToText = "";
            try
            {
                if(data.entry[0].messaging[0].message.attachments != null)
                {
                    imageToText = data.entry[0].messaging[0].message.attachments[0].payload.url;
                    string textResult = await ConvertImageURLToBase64(data.entry[0].messaging[0].message.attachments[0].payload.url);
                    var ctrlrDict = DependencyResolver.Current.GetService<DictionaryController>();
                    ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                    await ctrlrDict.searchDictViaBot(textResult, data.entry[0].messaging[0].sender.id, "");
                    return null;
                }
                if (data.entry[0].messaging[0].message.text != "" && data.entry[0].messaging[0].message.quick_reply == null)
                {
                    var ctrlrDict = DependencyResolver.Current.GetService<DictionaryController>();
                    ctrlrDict.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlrDict);
                    await ctrlrDict.searchDictViaBot(data.entry[0].messaging[0].message.text, data.entry[0].messaging[0].sender.id, "");
                    return null;
                }
            }
            catch
            {

            }
            var ctrlr = DependencyResolver.Current.GetService<LearningController>();
            ctrlr.ControllerContext = new ControllerContext(this.Request.RequestContext, ctrlr);
            ctrlr.ReceivePost(data);

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
    }
}
