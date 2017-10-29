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
    public class AdminCommentVoteDetailController : AdminBaseController<CommentVoteDetail, CommentVoteDetailInputAdminViewData>
    {
        ICommentVoteDetailService _commentVoteDetailService;
        public AdminCommentVoteDetailController(ICommentVoteDetailService commentVoteDetailService, ILayoutAdminService layoutService)
            : base(commentVoteDetailService, layoutService)
        {
            _commentVoteDetailService = commentVoteDetailService;
        }
        //
        // GET: /CommentVoteDetailAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<CommentVoteDetailInputAdminViewData> newPatientItems = new AutoForm<CommentVoteDetailInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
