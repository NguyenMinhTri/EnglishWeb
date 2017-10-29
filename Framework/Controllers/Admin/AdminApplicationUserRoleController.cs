using Framework.Model;
using Framework.ViewData.Admin.GetData;
using Framework.ViewData.Admin.Input;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Framework.Service.Admin;
using Framework.ViewData;
using Framework.Model.TableJoin;
using Framework.Common;
using Framework.ViewModels;
using Framework.Service;
using Framework.Models;

namespace Framework.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminApplicationUserRoleController : AdminBaseController<ApplicationUserRole,
        ApplicationUserRoleInputAdminViewData>
    {
        IApplicationUserRoleService _applicationUserRoleService;
        IComboboxService _comboboxService;
        IRoutingService _routingService;
        IApplicationUserService _userService;
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public AdminApplicationUserRoleController(IApplicationUserRoleService applicationUserRoleService,
            IComboboxService comboboxService,
            IApplicationUserService userService,
            IRoutingService routingService,
            ILayoutAdminService layoutService)
            : base(applicationUserRoleService, layoutService)
        {
            _applicationUserRoleService = applicationUserRoleService;
            _comboboxService = comboboxService;
            _routingService = routingService;
            _userService = userService;
            UserManager = new UserManager<ApplicationUser>(new ApplicationUserStore(new ApplicationDbContext()));
        }
        //
        // GET: /AdminApplicationUserRole/
        public ActionResult Index(String userId)
        {
            CreateLayoutAdminView();
            AutoForm<ApplicationUserRoleInputAdminViewData> applicationUserRoles =
                new AutoForm<ApplicationUserRoleInputAdminViewData>();
            var forms = applicationUserRoles.GetControls();
            ComboboxViewData roleCombobox = (ComboboxViewData)forms[1];
            roleCombobox.Url += "?userId=" + userId;
            ViewBag.Forms = forms;
            ViewBag.OtherId = userId;
            return View();
        }

        public override JsonResult Add(ApplicationUserRoleInputAdminViewData entity)
        {
            var result = base.Add(entity);

            ApplicationUserRole userRole = (ApplicationUserRole)result.Data;
            _routingService.Save();

            return result;
        }

        public override JsonResult Update(ApplicationUserRoleInputAdminViewData entity)
        {
            var model = _applicationUserRoleService.GetById(entity.Id);
            var user = _layoutService.GetUserById(model.UserId);
            var oldObj = _service.GetById(entity.Id);
            var url = SlugHelper.GenerateSlug(user.Name);
            user.Logined = false;
            _userService.Update(user);
            _userService.Save();
            _routingService.Save();
            var result = base.Update(entity);
            return result;
        }

        public override JsonResult Delete(ViewData.Admin.DeleteViewData obj)
        {
            var entity = _service.GetById(obj.Id);
            var user = _layoutService.GetUserById(entity.UserId);
            user.Logined = false;
            _userService.Update(user);
            var url = SlugHelper.GenerateSlug(user.Name);
            _applicationUserRoleService.Delete(obj.Id);
            _applicationUserRoleService.Save();
            return Json(new
            {
                result = "success"
            });
        }
        public JsonResult GetAllDisplay(String userId)
        {
            List<ApplicationUserRoleGetAll> allicationUserRoles = _applicationUserRoleService.GetAllWithDisplayByUserId(userId);

            List<ApplicationUserRoleInputAdminViewData> results = new List<ApplicationUserRoleInputAdminViewData>();
            foreach (var applicationUserRole in allicationUserRoles)
            {
                var userRole = PropertyCopy.Copy<ApplicationUserRoleInputAdminViewData, ApplicationUserRoleGetAll>(applicationUserRole);
                userRole.UserId = applicationUserRole.ApplicationUser_Id;
                results.Add(userRole);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetApplicationRoles(String userId)
        {
            return Json(_comboboxService.GetRolesExceptInUserIdCombobox(userId), JsonRequestBehavior.AllowGet);
        }

    }
}
