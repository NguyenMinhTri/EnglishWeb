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
    public class AdminOurWordController : AdminBaseController<OurWord, OurWordInputAdminViewData>
    {
        IOurWordService _ourWordService;
        public AdminOurWordController(IOurWordService ourWordService, ILayoutAdminService layoutService)
            : base(ourWordService, layoutService)
        {
            _ourWordService = ourWordService;
        }
        //
        // GET: /OurWordAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<OurWordInputAdminViewData> newPatientItems = new AutoForm<OurWordInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
