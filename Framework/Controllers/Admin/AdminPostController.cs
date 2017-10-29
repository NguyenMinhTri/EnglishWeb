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
    public class AdminPostController : AdminBaseController<Post, PostInputAdminViewData>
    {
        IPostService _postService;
        public AdminPostController(IPostService postService, ILayoutAdminService layoutService)
            : base(postService, layoutService)
        {
            _postService = postService;
        }
        //
        // GET: /PostAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<PostInputAdminViewData> newPatientItems = new AutoForm<PostInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
