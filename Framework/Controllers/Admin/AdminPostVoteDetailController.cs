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
    public class AdminPostVoteDetailController : AdminBaseController<PostVoteDetail, PostVoteDetailInputAdminViewData>
    {
        IPostVoteDetailService _postVoteDetailService;
        public AdminPostVoteDetailController(IPostVoteDetailService postVoteDetailService, ILayoutAdminService layoutService)
            : base(postVoteDetailService, layoutService)
        {
            _postVoteDetailService = postVoteDetailService;
        }
        //
        // GET: /PostVoteDetailAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<PostVoteDetailInputAdminViewData> newPatientItems = new AutoForm<PostVoteDetailInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
