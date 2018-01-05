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
using Microsoft.AspNet.Identity;
using Framework.Model;

namespace Framework.Controllers
{

    public class ProfileController : LayoutController
    {
        IClientFriendService _friendService;
        IClientProfileService _clientProfileService;
        IPostService _postService;
        IPostTypeService _postTypeService;
        IPostVoteDetailService _postVoteDetailService;
        ICommentService _commentOfPost;
        ICommentVoteDetailService _commentVoteDetailService;
        public ProfileController(ILayoutService layoutService,
            IClientProfileService clientProfileService,
            IPostService postService,
            IPostTypeService postTypeService,
            IPostVoteDetailService postVoteDetailService,
            ICommentService commentOfPost,
             ICommentVoteDetailService commentVoteDetailService,
            IClientFriendService friendService
            )
            : base(layoutService)
        {
            _clientProfileService = clientProfileService;
            _postService = postService;
            _postTypeService = postTypeService;
            _postVoteDetailService = postVoteDetailService;
            _commentOfPost = commentOfPost;
            _commentVoteDetailService = commentVoteDetailService;
            _friendService = friendService;
        }

        HeaderViewModel HeaderViewModel
        {
            get
            {
                return (HeaderViewModel)_viewModel;
            }
        }

        NewsFeedViewModel NewsFeedViewModel
        {
            get
            {
                return (NewsFeedViewModel)_viewModel;
            }
        }

        AboutViewModel AboutViewModel
        {
            get
            {
                return (AboutViewModel)_viewModel;
            }
        }

        FriendViewModel FriendViewModel
        {
            get
            {
                return (FriendViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option, string username)
        {

            _viewModel = new HeaderViewModel();

            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "newsfeed":
                        {
                            CreateLayoutView("Trang cá nhân");
                            break;
                        }
                    case "about":
                        {
                            CreateLayoutView("Thông tin");
                            break;
                        }
                    case "friend":
                        {
                            CreateLayoutView("Bạn bè");
                            break;
                        }
                    default:
                        {
                            CreateLayoutView("Trang cá nhân");
                            break;
                        }
                }
            }
            else
            {
                CreateLayoutView("Trang cá nhân");
            }
            if (username == null || username == "false")
            {
                username = _viewModel.User.UserName;
            }
            var user = _service.GetUserByUserName(username);
            FieldHelper.CopyNotNullValue(HeaderViewModel, user);
            HeaderViewModel.CodeRelationshipId = _friendService.FindRelationship(User.Identity.GetUserId(), user.Id);
            return View(_viewModel);
        }

        [HttpGet]
        public ActionResult NewsFeed(string username)
        {
            _viewModel = new NewsFeedViewModel();
            CreateLayoutView("Trang cá nhân");
            if (username == "false")
            {
                username = _viewModel.User.UserName;
            }
            ApplicationUser userCur = _service.GetUserByUserName(username);
            FieldHelper.CopyNotNullValue(NewsFeedViewModel, userCur);

            List<Post> listPost = new List<Post>();
            listPost = _postService.getPostByUser(userCur.Id).Take(5).ToList();
            NewsFeedViewModel.ListPostType = _postTypeService.GetAll().ToList();

            foreach (Post post in listPost)
            {
                PostViewModel postViewModel = new PostViewModel();
                ApplicationUser user = _service.GetUserById(post.Id_User);
                FieldHelper.CopyNotNullValue(postViewModel, user);
                FieldHelper.CopyNotNullValue(postViewModel, post);

                postViewModel.TypeToString = _postTypeService.GetById(post.Id_Type).Name;
                PostVoteDetail votePost = _postVoteDetailService.getVoteByIdUser(NewsFeedViewModel.Id, post.Id);
                if (votePost != null)
                {
                    postViewModel.Vote = votePost.Vote;
                }
                List<Comment> listComment = new List<Comment>();
                List<Comment> listChildComment = new List<Comment>();
                listComment = _commentOfPost.getCommentOfPost(post.Id);
                foreach (var parent in listComment)
                {
                    CommentViewModel commentViewModel = new CommentViewModel();
                    user = _service.GetUserById(parent.Id_User);
                    CommentVoteDetail voteComment = _commentVoteDetailService.getVoteByIdUser(NewsFeedViewModel.Id, parent.Id);
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
                        postViewModel.listComment.Insert(0, commentViewModel);
                    }
                    else
                    {
                        postViewModel.listComment.Add(commentViewModel);
                    }
                }
                NewsFeedViewModel.ListPost.Add(postViewModel);
            }

            return PartialView("_NewsFeed", NewsFeedViewModel);
        }

        public PartialViewResult MorePost(int page, string username)
        {
            MorePostViewModel morePostViewModel = new MorePostViewModel();
            ApplicationUser userCur = _service.GetUserByUserName(username);

            string id_user = User.Identity.GetUserId();
            List<Post> listPost = new List<Post>();
            listPost = _postService.getPostByUser(userCur.Id).Skip(page * 5).Take(5).ToList();

            foreach (Post post in listPost)
            {
                PostViewModel postViewModel = new PostViewModel();
                ApplicationUser user = _service.GetUserById(post.Id_User);
                FieldHelper.CopyNotNullValue(postViewModel, user);
                FieldHelper.CopyNotNullValue(postViewModel, post);

                postViewModel.TypeToString = _postTypeService.GetById(post.Id_Type).Name;
                PostVoteDetail votePost = _postVoteDetailService.getVoteByIdUser(id_user, post.Id);
                if (votePost != null)
                {
                    postViewModel.Vote = votePost.Vote;
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
                        postViewModel.listComment.Insert(0, commentViewModel);
                    }
                    else
                    {
                        postViewModel.listComment.Add(commentViewModel);
                    }
                }
                morePostViewModel.ListPost.Add(postViewModel);
                ApplicationUser userT = _service.GetUserById(id_user);
                morePostViewModel.UserId = userT.Id;
                morePostViewModel.Degree = userT.Degree;
                morePostViewModel.UserName = userT.UserName;
                morePostViewModel.Avatar = userT.Avatar;
            }
            return PartialView("_MorePost", morePostViewModel);
        }

        [HttpGet]
        public ActionResult About(string username)
        {
            _viewModel = new AboutViewModel();
            CreateLayoutView("Thông tin");
            if (username == "false")
            {
                username = _viewModel.User.UserName;
            }
            var user = _service.GetUserByUserName(username);
            FieldHelper.CopyNotNullValue(AboutViewModel, user);
            return PartialView("_About", AboutViewModel);
        }

        [HttpGet]
        public ActionResult Friend(string username)
        {
            _viewModel = new FriendViewModel();
            CreateLayoutView("Bạn bè");
            if (username == "false")
            {
                username = _viewModel.User.UserName;
            }
            var user = _service.GetUserByUserName(username);
            FieldHelper.CopyNotNullValue(FriendViewModel, user);
            return PartialView("_Friend", FriendViewModel);
        }
    }
}
