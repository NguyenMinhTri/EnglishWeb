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
    public class AdminVocaScheduleController : AdminBaseController<VocaSchedule, VocaScheduleInputAdminViewData>
    {
        IVocaScheduleService _vocaScheduleService;
        public AdminVocaScheduleController(IVocaScheduleService vocaScheduleService, ILayoutAdminService layoutService)
            : base(vocaScheduleService, layoutService)
        {
            _vocaScheduleService = vocaScheduleService;
        }
        //
        // GET: /VocaScheduleAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<VocaScheduleInputAdminViewData> newPatientItems = new AutoForm<VocaScheduleInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
