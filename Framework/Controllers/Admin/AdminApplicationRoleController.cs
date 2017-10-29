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
using Framework.ViewData.Admin;
using Framework.Service;

namespace Framework.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminApplicationRoleController : AdminBaseController<ApplicationRole, ApplicationRoleInputAdminViewData>
    {
        IApplicationRoleService _applicationRoleService;
        public AdminApplicationRoleController(IApplicationRoleService applicationRoleService, ILayoutAdminService layoutService
            )
            : base(applicationRoleService, layoutService)
        {
            _applicationRoleService = applicationRoleService;
        }
        //
        // GET: /AdminApplicationRole/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<ApplicationRoleInputAdminViewData> applicationRoles = new AutoForm<ApplicationRoleInputAdminViewData>();
            ViewBag.Forms = applicationRoles.GetControls();
            return View();
        }
        public override JsonResult Add(ApplicationRoleInputAdminViewData entity)
        {
            entity.Id = entity.Name;
            JsonResult result = base.Add(entity);
            

            return result;
        }
        public override JsonResult Delete(DeleteViewData obj)
        {
            return Json(new
            {
                result = "failed"
            });
        }
        [HttpPost]
        public JsonResult DeleteWithStringId(DeleteWithStringIdViewData obj)
        {
            _applicationRoleService.DeleteByStringId(obj.Id);
            _applicationRoleService.Save();
            return Json(new
            {
                result = "success"
            });
        }

    }
}
