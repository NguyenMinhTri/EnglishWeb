﻿using Framework.Service;
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
            List<PostCommentDetail> listComment = new List<PostCommentDetail>();
            List<PostCommentDetail> listChildComment = new List<PostCommentDetail>();
            listComment = _commentOfPost.getCommentOfPost(post.Id);
            foreach (var parent in listComment)
            {
                CommentViewModel commentViewModel = new CommentViewModel();
                user = _service.GetUserById(parent.Id_User);
                FieldHelper.CopyNotNullValue(commentViewModel, user);
                FieldHelper.CopyNotNullValue(commentViewModel, parent);
                listChildComment = _commentOfPost.getChildOfComment(parent.Id_Post, parent.Id);
                if (listChildComment.Count != 0)
                {
                    foreach (var child in listChildComment)
                    {
                        CommentViewModel commentChildViewModel = new CommentViewModel();
                        user = _service.GetUserById(child.Id_User);
                        FieldHelper.CopyNotNullValue(commentChildViewModel, user);
                        FieldHelper.CopyNotNullValue(commentChildViewModel, child);
                        commentViewModel.listChildComment.Add(commentChildViewModel);
                    }
                }
                PostViewModel.listComment.Add(commentViewModel);
            }
            return View(PostViewModel);
        }

        [HttpPost]
        public PartialViewResult Comment(CommentViewModel data)
        {
            _viewModel = new CommentViewModel();
            PostCommentDetail comment = new PostCommentDetail();
            FieldHelper.CopyNotNullValue(comment, data);
            comment.Corrected = false;
            _commentOfPost.Add(comment);
            _commentOfPost.Save();
            Post post = _postService.GetById(comment.Id_Post);
            post.Comment++;
            _postService.Update(post);
            _postService.Save();
            ApplicationUser user = _service.GetUserById(comment.Id_User);
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

        [HttpPost]
        public JsonResult VotePost(CommentViewModel data)
        {
            var userOfPost = _postService.GetById(data.Id_Post);
            //Nếu vote đó của tác giả câu hỏi thì
            if(data.Id_User == userOfPost.Id_User)
            {
                var commentOfPost = _commentOfPost.GetById(data.Id);
                commentOfPost.Corrected = true;
                _commentOfPost.Update(commentOfPost);
                _commentOfPost.Save();
            }
            if (data.Content != null)
            {
                try
                {
                    return Json(new
                    {
                        result = "success"
                    });
                }
                catch (Exception e)
                {
                    return Json(new
                    {
                        result = "failed",
                    });
                }
            }
            return Json(new
            {
                result = "failed",
            });
        }

    }
}