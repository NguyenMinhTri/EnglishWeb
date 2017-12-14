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
    public class AdminCommentController : AdminBaseController<Comment, CommentInputAdminViewData>
    {
        ICommentService _postCommentDetailService;
        public AdminCommentController(ICommentService postCommentDetailService, ILayoutAdminService layoutService)
            : base(postCommentDetailService, layoutService)
        {
            _postCommentDetailService = postCommentDetailService;
        }
        //
        // GET: /CommentAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<CommentInputAdminViewData> newPatientItems = new AutoForm<CommentInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
