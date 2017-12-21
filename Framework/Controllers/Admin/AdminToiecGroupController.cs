using Framework.Model;
using Framework.Service.Admin;
using Framework.ViewData.Admin.GetData;
using Framework.ViewData.Admin.Input;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminToiecGroupController : AdminBaseController<ToiecGroup, ToiecGroupInputAdminViewData>
    {
        IToiecGroupService _toiecGroupService;
        public AdminToiecGroupController(IToiecGroupService toiecGroupService, ILayoutAdminService layoutService)
            : base(toiecGroupService, layoutService)
        {
            _toiecGroupService = toiecGroupService;
        }
        //
        // GET: /ToiecGroupAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<ToiecGroupInputAdminViewData> newPatientItems = new AutoForm<ToiecGroupInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
