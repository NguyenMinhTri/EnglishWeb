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
    public class AdminCodeController : AdminBaseController<Code, CodeInputAdminViewData>
    {
        ICodeService _codeService;
        public AdminCodeController(ICodeService codeService, ILayoutAdminService layoutService)
            : base(codeService, layoutService)
        {
            _codeService = codeService;
        }
        //
        // GET: /CodeAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<CodeInputAdminViewData> newPatientItems = new AutoForm<CodeInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
