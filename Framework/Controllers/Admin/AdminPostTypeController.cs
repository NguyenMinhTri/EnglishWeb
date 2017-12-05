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
    public class AdminPostTypeController : AdminBaseController<PostType, PostTypeInputAdminViewData>
    {
        IPostTypeService _postTypeService;
        public AdminPostTypeController(IPostTypeService postTypeService, ILayoutAdminService layoutService)
            : base(postTypeService, layoutService)
        {
            _postTypeService = postTypeService;
        }
        //
        // GET: /PostTypeAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<PostTypeInputAdminViewData> newPatientItems = new AutoForm<PostTypeInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
