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
    public class AdminDetailUserTypeController : AdminBaseController<DetailUserType, DetailUserTypeInputAdminViewData>
    {
        IDetailUserTypeService _detailUserTypeService;
        public AdminDetailUserTypeController(IDetailUserTypeService detailUserTypeService, ILayoutAdminService layoutService)
            : base(detailUserTypeService, layoutService)
        {
            _detailUserTypeService = detailUserTypeService;
        }
        //
        // GET: /DetailUserTypeAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<DetailUserTypeInputAdminViewData> newPatientItems = new AutoForm<DetailUserTypeInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
