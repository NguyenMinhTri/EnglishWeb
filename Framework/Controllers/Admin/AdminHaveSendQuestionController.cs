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
    public class AdminHaveSendQuestionController : AdminBaseController<HaveSendQuestion, HaveSendQuestionInputAdminViewData>
    {
        IHaveSendQuestionService _haveSendQuestionService;
        public AdminHaveSendQuestionController(IHaveSendQuestionService haveSendQuestionService, ILayoutAdminService layoutService)
            : base(haveSendQuestionService, layoutService)
        {
            _haveSendQuestionService = haveSendQuestionService;
        }
        //
        // GET: /HaveSendQuestionAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<HaveSendQuestionInputAdminViewData> newPatientItems = new AutoForm<HaveSendQuestionInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
