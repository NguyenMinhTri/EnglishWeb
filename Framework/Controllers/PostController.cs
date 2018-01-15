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
using System.Net.Http;
using System.Threading.Tasks;
using Framework.Model.Google;

namespace Framework.Controllers
{

    public class PostController : LayoutController
    {
        IPostService _postService;
        IPostTypeService _postTypeService;
        IPostVoteDetailService _postVoteDetailService;
        ICommentVoteDetailService _commentVoteDetailService;
        ICommentService _commentOfPost;
        IToiecGroupService _fbService;
        IDetailUserTypeService _detailUserTypeService;
        IClientNotificationService _notificationService;
        public PostController(ILayoutService layoutService,
            IPostService postService,
            IPostTypeService postTypeService,
            IPostVoteDetailService postVoteDetailService,
            ICommentService commentOfPost,
            ICommentVoteDetailService commentVoteDetailService,
        IClientNotificationService notificationService,
            IDetailUserTypeService detailUserTypeService, IToiecGroupService fbService)
            : base(layoutService)
        {
            _postService = postService;
            _postTypeService = postTypeService;
            _postVoteDetailService = postVoteDetailService;
            _commentOfPost = commentOfPost;
            _commentVoteDetailService = commentVoteDetailService;
            _detailUserTypeService = detailUserTypeService;
            _notificationService = notificationService;
            _fbService = fbService;
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

        public ActionResult Index(int id, int? Comment)
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
            CommentViewModel commentCorrected = null;
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
                if (Comment != null && commentViewModel.Id.Equals(Comment))
                {
                    PostViewModel.listComment.Insert(0, commentViewModel);
                }
                else
                {
                    if (commentViewModel.Corrected)
                    {
                        commentCorrected = commentViewModel;
                    }
                    else
                    {
                        PostViewModel.listComment.Add(commentViewModel);
                    }
                }
            }
            if (commentCorrected != null)
            {
                PostViewModel.listComment.Insert(0, commentCorrected);
            }
            List<PostType> listPostType = _postTypeService.GetAll().ToList();
            List<PostTypeViewModel> listPostTypeViewModel = new List<PostTypeViewModel>();
            foreach (PostType item in listPostType)
            {
                PostTypeViewModel postType = new PostTypeViewModel();
                FieldHelper.CopyNotNullValue(postType, item);
                postType.Register = _detailUserTypeService.getRegisterPostType(id_user, item.Id);
                listPostTypeViewModel.Add(postType);
            }
            ViewBag.ListPostType = listPostTypeViewModel;
            return View(PostViewModel);
        }

        public PartialViewResult CommentSection(int id_comment)
        {
            Comment comment = new Comment();
            List<Comment> listChildComment = new List<Comment>();
            comment = _commentOfPost.GetById(id_comment);
            Post post = _postService.GetById(comment.Id_Post);
            CommentViewModel commentViewModel = new CommentViewModel();
            commentViewModel.Id_UserPost = post.Id_User;
            ApplicationUser user = _service.GetUserById(comment.Id_User);
            CommentVoteDetail voteComment = _commentVoteDetailService.getVoteByIdUser(User.Identity.GetUserId(), comment.Id);
            if (voteComment != null)
            {
                commentViewModel.Vote = voteComment.Vote;
            }
            FieldHelper.CopyNotNullValue(commentViewModel, user);
            FieldHelper.CopyNotNullValue(commentViewModel, comment);
            listChildComment = _commentOfPost.getChildOfComment(comment.Id_Post, comment.Id);
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
            return PartialView("_Comment", commentViewModel);
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
            CommentViewModel.Id_UserPost = post.Id_User;
            if (data.Id_Comment == 0)
            {
                if (CommentViewModel.Id_UserPost != User.Identity.GetUserId())
                {
                    Notification notification = new Notification();
                    notification.Id_User = data.Id_User;
                    notification.Id_Friend = CommentViewModel.Id_UserPost;
                    notification.Id_Post = data.Id_Post;
                    notification.Id_Comment = comment.Id;
                    notification.DateComment = data.DateComment;
                    _notificationService.Add(notification);
                    _notificationService.Save();
                }
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
        [AllowAnonymous]
        public async Task CheckAnswerOnFB()
        {

            //Lay danh sách cac bài post chưa có câu trả lời
            //  var listPost = _postService.GetAll().Where(x => x.Id_PostFB != null && x.Post_Status == 0);
            var listPost = _postService.checkPostOnGroup();
            foreach (var post in listPost)
            {
                var dapAn = await _fbService.DetectedAnswOfPost(post.Id_PostFB);
                if (dapAn != "")
                {
                    post.Post_Status = 10;
                    Comment comment = new Comment();
                    comment.Corrected = false;
                    comment.Content = dapAn;
                    //Admin
                    comment.Id_User = "eddf2dd7-5cfd-4828-904d-ed686780327a";
                    comment.Id_Post = post.Id;
                    comment.DateComment = DateTime.Now.Ticks.ToString();
                    _commentOfPost.Add(comment);
                    _commentOfPost.Save();
                    _postService.Update(post);
                    _postService.Save();
                    await notifyForUserAboutQues(post.Id);
                }
            }
        }
        //Thông báo cho người đăng câu hỏi là đã có đáp án
        public async Task<string> notifyForUserAboutQues(int idQues)
        {
            //Thông báo cho người đặt câu hỏi
            var currentPost = _postService.GetById(idQues);
            var detailPost = _commentOfPost.GetAll().Where(x => x.Id_Post == idQues).LastOrDefault();
            var userOfPost = _service.GetUserById(currentPost.Id_User);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + userOfPost.Id_Messenger + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a2c0352e4b0d0e6161fc7a1";
            paramChatfuel += "&traloicauhoi=" + currentPost.Content;
            paramChatfuel += "&dapancauhoi=" + detailPost.Content;
            var response2 = await client.PostAsync(paramChatfuel, null);
            return "";
        }
    }
}