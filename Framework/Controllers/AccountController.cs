using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Framework.Models;
using Framework.Model;
using Framework.Service.Admin;
using System.Drawing;
using System.IO;
using Framework.Common;
using Framework.Model.Google;
using Newtonsoft.Json;

namespace Framework.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IApplicationUserService _applicationUserService;
        IApplicationUserRoleService _userRolesService;
        public AccountController(IApplicationUserService applicationUserService, IApplicationUserRoleService userRolesService)
            : this(new UserManager<ApplicationUser>(new ApplicationUserStore(new ApplicationDbContext())))
        {
            _applicationUserService = applicationUserService;
            _userRolesService = userRolesService;
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new ApplicationUserStore(ApplicationDbContext));
        }

        public ApplicationDbContext ApplicationDbContext
        {
            private set;
            get;
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.EmailConfirmed = true;
                    await UserManager.UpdateAsync(user);
                    await SignInAsync(user, isPersistent: false);
                    return Redirect("/YourAccount?option=updateinfo&userType=new");
                }
                else
                {
                    return RedirectToAction("ConfirmEmail", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("ConfirmEmail", "Account", new { Email = "" });
            }
        }

        public ActionResult ResendMail(String emailAddress, String name, String userId)
        {
            var u = Url.Action("ConfirmEmail", "Account", new { Token = userId, Email = emailAddress}, Request.Url.Scheme);
            u = u.Replace("s-/", "");

            MailHelper.SendMail(emailAddress, name, "Email confirmation", string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>",
            emailAddress,
            u));

            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.CurrentExecutionFilePath.ToLower().Contains("account"))
                {
                    return Redirect(System.Web.Configuration.WebConfigurationManager.AppSettings["RolesAuthRedirectUrl"]);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    result = "failed",
                    data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
                });
            }

            var user = UserManager.Find(model.Email, model.Password);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("Email",
                        String.Format("Hình như có ai đó chưa kích hoạt tài khoản..."));
                    return Json(new
                    {
                        result = "failed",
                        data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
                    });
                }
                if (!user.Status)
                {
                    ModelState.AddModelError("Email", "Tài khoản sao bị khóa rồi. Liên hệ với Olympus ngay nha!!!");
                    return Json(new
                    {
                        result = "failed",
                        data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
                    });
                }

                await SignInAsync(user, model.RememberMe);
                user.Logined = true;
                await UserManager.UpdateAsync(user);
                return Json(new
                {
                    result = "success"
                });
            }
            else
            {
                ModelState.AddModelError("Email", "Sai Email hay mật khẩu gì rồi bạn eiii");
                return Json(new
                {
                    result = "failed",
                    data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
                });
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (System.Data.Entity.DbContextTransaction dbTran = ApplicationDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new ApplicationUser() { UserName = model.Email };
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Birthday = model.Birthday;
                        user.Sex = model.Sex;
                        user.CreatedDate = DateTime.Now;
                        user.Logined = true;
                        user.Status = true;
                        user.Avatar = "avatar1.jpg";
                        user.Degree = "tieuhoc";

                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            var u = Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme);
                            u = u.Replace("s-/", "");
                            u += "&userType=new";
                            MailHelper.SendMail(user.Email, user.Name, "Xác nhận thành viên của Olympus", string.Format("<div style='border-collapse: separate;width: 100%;height: 100%;display: flex;'> <div class='content' style='box-sizing: border-box;display: block;Margin: auto;max-width: 700px;padding: 10px;'> <table class='main' style='border-collapse: separate;mso-table-lspace: 0pt;mso-table-rspace: 0pt;width: 100%;border-radius: 5px;'> <tbody><tr> <td class='wrapper' style='font-family: sans-serif;font-size: 14px;vertical-align: top;box-sizing: border-box;border: 2px solid #ff5e3a;border-radius: 5px;background: white;'> <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%;'> <tbody><tr> <td style='font-family: sans-serif;font-size: 14px;=: 100%overflow:;'> <div style='margin: -1px;padding: 10px 20px;background: #ff5e3a;'> <h1 style='font-family: sans-serif;font-size: 20px;font-weight: normal;color: white;float: left;'>Chào mừng bạn đến với Olympus!!!</h1> <a href='http://olympusenglish.azurewebsites.net' style='text-align: right; display: block;'> <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACoAAAAuCAQAAAD0W0PGAAAB+0lEQVR42u3VM7SERxwH0F9s20Yb27YtdbFto41dxbZt27Zt7M05+/QtZpHXvjv96K+MGVPiBJ38aqb0z+M6OSW9cpKfnZQkpvGPsu9Nk175Ez8nic10ckTyf256rrIvTZYkpk5/vK9sn9S5z2bpnQWUfWiiJLEyvjdH92cPsruy3VLnanBd9wANcoOSN42fOiupgeV7uqnx/aBkqwxzKbghvbCckueNU/15Nfxl+nTnWCXrJVUeBrummcnSxKPae8W8g2ue1DkaXJJmHnCsSTLMVP7RzQmpsyJ4Js18jXcNb2tT3dxi3NSZCXzdPp2YOIOco7M3TFXJE/ir+6bv6uQHC2eYCcE/aeZDMHPaMB+qatZPhVlKz38YrJQ27I2qo5Iqq4Bn08yZ4MC04U5V1xonqXIKODfNbAseTAuT+4NhL5s8TbwEtk8zU/sDNQuniY0Z9p3508Qi4E/Tp5WrwMVp4gIG/WOttHAZuCrtWBz8a8lUGMenUPpxy6uBFdKea8HrpsowizLo8rThXHBLSszmR3CvCTPIUYDnTJo2zOBL35k7ZbZUA3ebMnWeAF+ZKwU2t0k6cyj43TxJYkb/4m8rZ3QcpuaY1NkJ7F0cPL2zvokraXZJhxHZPxP5wZMmarld+abdWdPnZm+53eg4w3It/zhaVs2YMX37D23hC0V5O/45AAAAAElFTkSuQmCC' alt='Olympus'> </a> </div> <p style='font-family: sans-serif;font-size: 16px;font-weight: normal;margin: 0;Margin-bottom: 15px;line-height: 25px;padding: 35px 35px 10px 35px;text-align: justify;'>Chào bạn,<br/> {0} biết không, chỉ cần click vào nút xác nhận bên dưới là giờ bạn đã có thể trở thành 1 phần trong đại gia đình Olympus rồi đó!!! Mau lên nào, chúng tôi không thể đợi lâu hơn nữa!!!!</p> <table border='0' cellpadding='0' cellspacing='0' class='btn btn-primary' style='border-collapse: separate;mso-table-lspace: 0pt;mso-table-rspace: 0pt;box-sizing: border-box;margin: auto;'> <tbody> <tr> <td align='left' style='font-family: sans-serif;font-size: 14px;vertical-align: top;'> <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: auto;'> <tbody> <tr> <td style='font-family: sans-serif;font-size: 14px;vertical-align: top;border-radius: 5px;text-align: center;'> <a href='{1}' title='Xác nhận thành viên Olympus' target='_blank' style='display: inline-block; color: rgb(255, 255, 255); background: rgb(255, 94, 58); border-radius: 5px; box-sizing: border-box; cursor: pointer; text-decoration: none; font-size: 14px; font-weight: bold; margin: 0px; padding: 12px 25px; text-transform: capitalize;' onmouseover='this.style.background=' #ff763a''='' onmouseout='this.style.background=' #ff5e3a''=''>Click vô!!!</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> <p class='apple-link' style='color: #999999;font-size: 12px;line-height: 20px;margin: 0px;padding: 35px 35px 35px 35px;'>Đây là email được gửi tự động từ hệ thống Olympus! Bạn không cần phải trả lời đâu nhé!!! Xác nhận!</p> </td> </tr> </tbody></table> </td> </tr> </tbody></table> </div> </div>",
                            user.LastName,
                            u));
                            dbTran.Commit();
                            return Json(new
                            {
                                result = "success"
                            });
                        }
                        else
                        {
                            dbTran.Rollback();
                            ModelState.AddModelError("Email", "Đã tồn tại email này rồiiii");
                            return Json(new
                            {
                                result = "failed",
                                data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        dbTran.Rollback();
                        throw e;
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Json(new
            {
                result = "failed",
                data = ModelState.Select(x => new { Field = x.Key, Error = x.Value.Errors }).ToList()
            });
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {

                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult LogOffDialog()
        {
            AuthenticationManager.SignOut();
            return Json(new { result = "success" });
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        // GET: /Account/Register
        [AllowAnonymous]
        public void registerChatBot(string idUser, string idmessenger)
        {
            
            var user = _applicationUserService.GetUserById(idUser);
            if(user == null)
            {

                ChatBotMessenger.sendTextMeg(idmessenger, "ID không tồn tại vui lòng kiểm tra lại");
                return ;
            }
            user.Id_Messenger = idmessenger;
            _applicationUserService.Update(user);
            _applicationUserService.Save();
            ChatBotMessenger.sendTextMeg(idmessenger, "Chúc mừng bạn đã liên kết thành công");
            return;
        }
    }
}