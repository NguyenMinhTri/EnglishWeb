using Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Service.Admin;
using Framework.Common;
using System.Threading;
using System.Web.UI;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Framework.ViewModels;
using Framework.Service.Client;
using Framework.Model;

namespace Framework.Controllers
{

    public class PostController : LayoutController
    {
        IPostService _postService;
        IPostTypeService _postTypeService;
        IPostVoteDetailService _postVoteDetailService;
        IPostCommentDetailService _commentOfPost;

        public PostController(ILayoutService layoutService,
            IPostService postService,
            IPostTypeService postTypeService,
            IPostVoteDetailService postVoteDetailService,
             IPostCommentDetailService commentOfPost
            )
            : base(layoutService)
        {
            _postService = postService;
            _postTypeService = postTypeService;
            _postVoteDetailService = postVoteDetailService;
            _commentOfPost = commentOfPost;
        }

        PostViewModel PostViewModel
        {
            get
            {
                return (PostViewModel)_viewModel;
            }
        }

        CommentViewModel CommentViewModel
        {
            get
            {
                return (CommentViewModel)_viewModel;
            }
        }

        public ActionResult Index(int id)
        {
            _viewModel = new PostViewModel();
            CreateLayoutView("Trả lời câu hỏi");
            Post post = _postService.GetById(id);
            ApplicationUser user = _service.GetUserById(post.Id_User);
            FieldHelper.CopyNotNullValue(PostViewModel, user);
            FieldHelper.CopyNotNullValue(PostViewModel, post);
            PostViewModel.TypeToString = _postTypeService.GetById(post.Id_Type).Name;
            PostVoteDetail vote = _postVoteDetailService.getVoteByIdUser(post.Id_User);
            if (vote != null)
            {
                PostViewModel.Vote = vote.Vote;
            }
            return View(PostViewModel);
        }

        [HttpPost]
        public PartialViewResult Comment(CommentViewModel data)
        {
            _viewModel = new CommentViewModel();
            PostCommentDetail comment = new PostCommentDetail();
            FieldHelper.CopyNotNullValue(comment, data);
            _commentOfPost.Add(comment);
            _commentOfPost.Save();

            ApplicationUser user = _service.GetUserById(comment.Id_Friend);
            FieldHelper.CopyNotNullValue(CommentViewModel, user);
            FieldHelper.CopyNotNullValue(CommentViewModel, comment);

            if (data.Id_Comment == 0)
            {
                return PartialView("_Comment", CommentViewModel);
            }
            else
            {
                return PartialView("_ChildComment", CommentViewModel);
            }
        }
    }
}