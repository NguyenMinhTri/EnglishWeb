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
    public class AdminDictCacheController : AdminBaseController<DictCache, DictCacheInputAdminViewData>
    {
        IDictCacheService _dictCacheService;
        public AdminDictCacheController(IDictCacheService dictCacheService, ILayoutAdminService layoutService)
            : base(dictCacheService, layoutService)
        {
            _dictCacheService = dictCacheService;
        }
        //
        // GET: /DictCacheAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<DictCacheInputAdminViewData> newPatientItems = new AutoForm<DictCacheInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
