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

namespace Framework.Controllers
{

    public class YourAccountController : LayoutController
    {
        IApplicationUserService _applicationUserService;
        public YourAccountController(ILayoutService layoutService,
            IApplicationUserService applicationUserService)
            : base(layoutService)
        {
            _applicationUserService = applicationUserService;
            UserManager = new UserManager<ApplicationUser>(new ApplicationUserStore(new ApplicationDbContext()));
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
        NotificationViewModel NotificationViewModel
        {
            get
            {
                return (NotificationViewModel)_viewModel;
            }
        }

        MessengerViewModel MessengerViewModel
        {
            get
            {
                return (MessengerViewModel)_viewModel;
            }
        }

        RequestsViewModel RequestsViewModel
        {
            get
            {
                return (RequestsViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option)
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
                    case "messenger":
                        {
                            _viewModel = new MessengerViewModel();
                            CreateLayoutView("Tin nhắn");
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

            return View(_viewModel);
        }

        [HttpGet]
        public ActionResult UpdateInfo()
        {
            _viewModel = new UpdateInfoViewModel();
            CreateLayoutView("Cập nhật thông tin");
            FieldHelper.CopyNotNullValue(UpdateInfoViewModel, _viewModel.User);
            return PartialView("_UpdateInfo", UpdateInfoViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateInfo(UpdateInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _service.GetUserById(User.Identity.GetUserId());
                FieldHelper.CopyNotNullValue(user, model);

                _applicationUserService.Update(user);
                try
                {
                    _applicationUserService.Save();
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
            return PartialView("_UpdateSetting");
        }

        [HttpGet]
        public ActionResult Notification()
        {
            _viewModel = new NotificationViewModel();
            return PartialView("_Notification", NotificationViewModel);
        }

        [HttpGet]
        public ActionResult Messenger()
        {
            _viewModel = new MessengerViewModel();
            return PartialView("_Messenger", MessengerViewModel);
        }

        [HttpGet]
        public ActionResult Requests()
        {
            _viewModel = new RequestsViewModel();
            return PartialView("_Requests", RequestsViewModel);
        }
    }
}
