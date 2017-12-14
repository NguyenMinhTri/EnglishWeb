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
using Microsoft.AspNet.Identity;

namespace Framework.Controllers
{

    public class PostController : LayoutController
    {
        IPostService _postService;
        IPostTypeService _postTypeService;
        IPostVoteDetailService _postVoteDetailService;
        ICommentVoteDetailService _commentVoteDetailService;
        ICommentService _commentOfPost;

        public PostController(ILayoutService layoutService,
            IPostService postService,
            IPostTypeService postTypeService,
            IPostVoteDetailService postVoteDetailService,
             ICommentService commentOfPost,
            ICommentVoteDetailService commentVoteDetailService
            )
            : base(layoutService)
        {
            _postService = postService;
            _postTypeService = postTypeService;
            _postVoteDetailService = postVoteDetailService;
            _commentOfPost = commentOfPost;
            _commentVoteDetailService = commentVoteDetailService;
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
            string id_user = User.Identity.GetUserId();
            _viewModel = new PostViewModel();
            CreateLayoutView("Trả lời câu hỏi");
            Post post = _postService.GetById(id);
            ApplicationUser user = _service.GetUserById(post.Id_User);
            FieldHelper.CopyNotNullValue(PostViewModel, user);
            FieldHelper.CopyNotNullValue(PostViewModel, post);
            PostViewModel.TypeToString = _postTypeService.GetById(post.Id_Type).Name;
            PostVoteDetail votePost = _postVoteDetailService.getVoteByIdUser(id_user, post.Id);
            if (votePost != null)
            {
                PostViewModel.Vote = votePost.Vote;
            }
            List<Comment> listComment = new List<Comment>();
            List<Comment> listChildComment = new List<Comment>();
            listComment = _commentOfPost.getCommentOfPost(post.Id);
            foreach (var parent in listComment)
            {
                CommentViewModel commentViewModel = new CommentViewModel();
                user = _service.GetUserById(parent.Id_User);
                CommentVoteDetail voteComment = _commentVoteDetailService.getVoteByIdUser(id_user, parent.Id);
                if (voteComment != null)
                {
                    commentViewModel.Vote = voteComment.Vote;
                }
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
                if (commentViewModel.Corrected)
                {
                    PostViewModel.listComment.Insert(0, commentViewModel);
                }
                else
                {
                    PostViewModel.listComment.Add(commentViewModel);
                }
            }
            return View(PostViewModel);
        }

        [HttpPost]
        public PartialViewResult Comment(CommentViewModel data)
        {
            _viewModel = new CommentViewModel();
            Comment comment = new Comment();
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
        public JsonResult VotePost(VoteViewModel data)
        {
            if (data.Id_Comment == 0)
            {
                Post post = _postService.GetById(data.Id_Post);
                PostVoteDetail vote;
                vote = _postVoteDetailService.getVoteByIdUser(data.Id_User, data.Id_Post);
                if (vote != null)
                {
                    if (data.Vote == vote.Vote)
                    {
                        vote.Vote = 0;
                    }
                    else
                    {
                        vote.Vote = data.Vote;
                    }
                    post.UpVote += data.UpVote;
                    post.DownVote += data.DownVote;
                    if (post.UpVote < 0)
                    {
                        post.UpVote = 0;
                    }
                    if (post.DownVote < 0)
                    {
                        post.DownVote = 0;
                    }
                    _postVoteDetailService.Update(vote);
                }
                else
                {
                    vote = new PostVoteDetail();
                    FieldHelper.CopyNotNullValue(vote, data);
                    _postVoteDetailService.Add(vote);
                    if (vote.Vote > 0)
                    {
                        post.UpVote++;
                    }
                    else
                    {
                        post.DownVote++;
                    }
                }
                _postService.Update(post);
                try
                {
                    _postVoteDetailService.Save();
                    _postService.Save();
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
            else
            {
                Comment comment = _commentOfPost.GetById(data.Id_Comment);
                CommentVoteDetail vote;
                vote = _commentVoteDetailService.getVoteByIdUser(data.Id_User, data.Id_Comment);
                if (vote != null)
                {
                    if (data.Vote == vote.Vote)
                    {
                        vote.Vote = 0;
                    }
                    else
                    {
                        vote.Vote = data.Vote;
                    }
                    comment.UpVote += data.UpVote;
                    comment.DownVote += data.DownVote;
                    if (comment.UpVote < 0)
                    {
                        comment.UpVote = 0;
                    }
                    if (comment.DownVote < 0)
                    {
                        comment.DownVote = 0;
                    }
                    _commentVoteDetailService.Update(vote);
                }
                else
                {
                    vote = new CommentVoteDetail();
                    FieldHelper.CopyNotNullValue(vote, data);
                    _commentVoteDetailService.Add(vote);
                    if (vote.Vote > 0)
                    {
                        comment.UpVote++;
                    }
                    else
                    {
                        comment.DownVote++;
                    }
                }
                _commentOfPost.Update(comment);
                try
                {
                    _commentVoteDetailService.Save();
                    _commentOfPost.Save();
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

        }

        [HttpPost]
        public JsonResult MarkAnswer(MarkAnswerViewModel data)
        {
            if (data.Id_Comment != 0 && _postService.GetById(data.Id_Post).Id_User == data.Id_User)
            {
                Comment oldCorrectComment = _commentOfPost.getCorrectComment(data.Id_Post);
                if (oldCorrectComment != null)
                {
                    oldCorrectComment.Corrected = false;
                    _commentOfPost.Update(oldCorrectComment);
                }
                Comment comment = _commentOfPost.GetById(data.Id_Comment);
                comment.Corrected = data.Corrected;
                _commentOfPost.Update(comment);
                try
                {
                    _commentOfPost.Save();
                    return Json(new
                    {
                        result = "success",
                        answer = "p" + data.Id_Post + "c-" + data.Id_Comment
                    });
                }
                catch (Exception e)
                {
                    return Json(new
                    {
                        result = "failed"
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