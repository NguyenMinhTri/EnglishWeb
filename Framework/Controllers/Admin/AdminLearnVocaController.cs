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
    public class AdminLearnVocaController : AdminBaseController<LearnVoca, LearnVocaInputAdminViewData>
    {
        ILearnVocaService _learnVocaService;
        public AdminLearnVocaController(ILearnVocaService learnVocaService, ILayoutAdminService layoutService)
            : base(learnVocaService, layoutService)
        {
            _learnVocaService = learnVocaService;
        }
        //
        // GET: /LearnVocaAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<LearnVocaInputAdminViewData> newPatientItems = new AutoForm<LearnVocaInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
