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
    public class AdminPostCommentDetailController : AdminBaseController<PostCommentDetail, PostCommentDetailInputAdminViewData>
    {
        IPostCommentDetailService _postCommentDetailService;
        public AdminPostCommentDetailController(IPostCommentDetailService postCommentDetailService, ILayoutAdminService layoutService)
            : base(postCommentDetailService, layoutService)
        {
            _postCommentDetailService = postCommentDetailService;
        }
        //
        // GET: /PostCommentDetailAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<PostCommentDetailInputAdminViewData> newPatientItems = new AutoForm<PostCommentDetailInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
