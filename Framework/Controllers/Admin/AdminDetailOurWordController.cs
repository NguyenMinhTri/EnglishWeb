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
    public class AdminDetailOurWordController : AdminBaseController<DetailOurWord, DetailOurWordInputAdminViewData>
    {
        IDetailOurWordService _detailOurWordService;
        public AdminDetailOurWordController(IDetailOurWordService detailOurWordService, ILayoutAdminService layoutService)
            : base(detailOurWordService, layoutService)
        {
            _detailOurWordService = detailOurWordService;
        }
        //
        // GET: /DetailOurWordAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<DetailOurWordInputAdminViewData> newPatientItems = new AutoForm<DetailOurWordInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
