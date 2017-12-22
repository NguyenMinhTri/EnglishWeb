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
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Framework.Model.Google;
using Newtonsoft.Json;

namespace Framework.Controllers
{

    public class HomeController : LayoutController
    {
        IClientHomeService _clientHomeService;
        IPostService _postService;
        IDetailUserTypeService _detailUserTypeService;
        IHaveSendQuestionService _haveSendQuesService;
        ICommentService _commentOfPost;
        IPostTypeService _postTypeService;
        IPostVoteDetailService _postVoteDetailService;
        ISubTypeService _subType;
        IDetailUserTypeService _detailUserType;
        ICommentVoteDetailService _commentVoteDetailService;
        IToiecGroupService _fbService;
        public HomeController(ILayoutService layoutService,
            IClientHomeService clientHomeService,
            IPostService postService,
            IDetailUserTypeService detailUser,
            IHaveSendQuestionService haveSendQuesService,
            IPostTypeService postTypeService,
            ICommentService commentOfPost,
            IPostVoteDetailService postVoteDetailService,
            ISubTypeService subType,
            IDetailUserTypeService detailUserType,
            ICommentVoteDetailService commentVoteDetailService,
            IToiecGroupService fbService
            )
            : base(layoutService)
        {
            _clientHomeService = clientHomeService;
            _postService = postService;
            _detailUserTypeService = detailUser;
            _haveSendQuesService = haveSendQuesService;
            _postTypeService = postTypeService;
            _commentOfPost = commentOfPost;
            _postVoteDetailService = postVoteDetailService;
            _subType = subType;
            _detailUserType = detailUserType;
            _commentVoteDetailService = commentVoteDetailService;
            _fbService = fbService;
        }

        HomeViewModel HomeViewModel
        {
            get
            {
                return (HomeViewModel)_viewModel;
            }
        }

        CommentViewModel CommentViewModel
        {
            get
            {
                return (CommentViewModel)_viewModel;
            }
        }

        PostViewModel PostViewModel
        {
            get
            {
                return (PostViewModel)_viewModel;
            }
        }

        public ActionResult Index(PostViewModel data, string userType, int? type)
        {
            _viewModel = new HomeViewModel();
            string id_user = User.Identity.GetUserId();
            List<Post> listPost = new List<Post>();
            if (type == null)
            {
                CreateLayoutView("Trang chủ");
                listPost = _postService.getPostByUserType(id_user).Take(5).ToList();
            }
            else
            {
                CreateLayoutView("Thể loại: " + _postTypeService.GetById(type.GetValueOrDefault()).Name);
                listPost = _postService.getPostByType(type.GetValueOrDefault()).Take(5).ToList();
            }
            List<PostType> listPostType = _postTypeService.GetAll().ToList();
            foreach (PostType item in listPostType)
            {
                PostTypeViewModel postType = new PostTypeViewModel();
                FieldHelper.CopyNotNullValue(postType, item);
                postType.Register = _detailUserTypeService.getRegisterPostType(id_user, item.Id);
                HomeViewModel.ListPostType.Add(postType);
            }
            ViewBag.newMember = userType;
            ViewBag.type = type.GetValueOrDefault();

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
                HomeViewModel.ListPost.Add(postViewModel);
            }
            return View(HomeViewModel);
        }

        public PartialViewResult MorePost(int page, int? type)
        {
            MorePostViewModel morePostViewModel = new MorePostViewModel();
            string id_user = User.Identity.GetUserId();
            List<Post> listPost = new List<Post>();
            if (type == 0)
            {
                listPost = _postService.getPostByUserType(id_user).Skip(page * 5).Take(5).ToList();
            }
            else
            {
                listPost = _postService.getPostByType(type.GetValueOrDefault()).Skip(page * 5).Take(5).ToList();
            }
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

        [HttpPost]
        public PartialViewResult Comment(CommentViewModel data)
        {
            _viewModel = new CommentViewModel();
            Comment comment = new Comment();
            FieldHelper.CopyNotNullValue(comment, data);
            _commentOfPost.Add(comment);
            _commentOfPost.Save();
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
        public async System.Threading.Tasks.Task<PartialViewResult> Post(PostViewModel data)
        {
            Post newPost = new Post();
            newPost.Post_Status = 0;
            _viewModel = new PostViewModel();
            FieldHelper.CopyNotNullValue(newPost, data);
            newPost.CreatedDate = DateTime.Now;
            if (data.Option == 0 && newPost.Id_Type == 8)
            {
                //post to fb toiec
                var IdPost = await _fbService.PostingToGroupFB(newPost.Content);
                newPost.Id_PostFB = IdPost.id;
            }
            _postService.Add(newPost);
            _postService.Save();
            string url = MaHoaMD5.Encrypt(newPost.Id + "#" + newPost.UpdatedDate);
            if (data.Option != 0)
            {
                ApplicationUser user = getExpertUserBasedOnType(newPost).FirstOrDefault();
                if (user != null)
                {
                    await sendNofityToMessenger(newPost, user);
                    HaveSendQuestion haveSendQues = new HaveSendQuestion();
                    haveSendQues.QuesID = newPost.Id;
                    haveSendQues.UserID = user.Id;
                    haveSendQues.Status = false;
                    haveSendQues.Protected = false;
                    _haveSendQuesService.Add(haveSendQues);
                    _haveSendQuesService.Save();
                }
            }
            else
            {
                //Send thong cho người dùng đăng ký loại câu hỏi đó

                List<ApplicationUser> userSubQues = getNormalUserBasedOnType(newPost);
                foreach (var itemUser in userSubQues)
                {
                    await sendNofityToMessenger(newPost, itemUser);
                    HaveSendQuestion haveSendQues = new HaveSendQuestion();
                    haveSendQues.QuesID = newPost.Id;
                    haveSendQues.UserID = itemUser.Id;
                    haveSendQues.Status = false;
                    haveSendQues.Protected = false;
                    _haveSendQuesService.Add(haveSendQues);
                    _haveSendQuesService.Save();
                }


            }
            //Send notify
            ApplicationUser userPost = _service.GetUserById(newPost.Id_User);
            FieldHelper.CopyNotNullValue(PostViewModel, userPost);
            FieldHelper.CopyNotNullValue(PostViewModel, newPost);
            PostViewModel.TypeToString = _postTypeService.GetById(newPost.Id_Type).Name;
            PostVoteDetail vote = _postVoteDetailService.getVoteByIdUser(newPost.Id_User, newPost.Id);
            if (vote != null)
            {
                PostViewModel.Vote = vote.Vote;
            }

            return PartialView("_Post", PostViewModel);
        }
        //Replay a question
        [HttpGet]
        public string Replay(string hashcode)
        {
            bool checkGio = false;
            int idQuestion = int.Parse(hashcode);
            var currentPost = _postService.GetById(int.Parse(hashcode));
            var userId = User.Identity.GetUserId();
            //Kiem tra xem con thoi gian tra loi hay ko
            var sendQues = _haveSendQuesService.GetAll().Where(x => x.QuesID == idQuestion && x.UserID == userId).ToList().FirstOrDefault();
            if (sendQues != null)
            {
                var timePost = sendQues.CreatedDate.Value.AddMinutes(TimeSetting.LimitMinuteForPost()).Ticks;
                var timeCurrent = DateTime.Now.Ticks;
                if (timePost < timeCurrent)
                {
                    checkGio = true;
                }

            }
            //
            //  currentPost.Post_Status = 1;
            // _postService.Update(currentPost);
            //_postService.Save();
            // DateTime 
            return checkGio ? "Vui lòng trả lời" : "Link hết hạn";
        }

        //Get tat ca cac post co thoi gian tao it hon hien tai 5'
        protected List<Post> getPost()
        {
            List<Post> listResult = new List<Post>();
            List<Post> listPost = _postService.GetAll().Where(post => post.Post_Status == 0 && post.Option == 1).ToList();
            foreach (var post in listPost)
            {
                var timePost = post.CreatedDate.Value.AddMinutes(TimeSetting.LimitMinuteForPost()).Ticks;
                var timeCurrent = DateTime.Now.Ticks;
                if (timePost < timeCurrent)
                {
                    listResult.Add(post);
                }
            }
            return listResult;
        }
        //Check time 5' and send this post for other expert
        [AllowAnonymous]
        public async Task checkTimeOfPost()
        {
            List<Post> listPost = getPost();
            //
            foreach (var post in listPost)
            {
                //Get expert user list 
                List<ApplicationUser> appUser = getExpertUserBasedOnType(post);
                //Update status

                //
                foreach (var expertUser in appUser)
                {
                    if (_haveSendQuesService.GetAll().Where(x => x.QuesID == post.Id && x.UserID == expertUser.Id).ToList().Count == 0)
                    {
                        post.Status = false; // have send
                        post.CreatedDate = DateTime.Now;
                        _postService.Update(post);
                        _postService.Save();
                        await Task.Delay(300);
                        await sendNofityToMessenger(post, expertUser);
                        HaveSendQuestion haveSendQues = new HaveSendQuestion();
                        haveSendQues.QuesID = post.Id;
                        haveSendQues.UserID = expertUser.Id;
                        haveSendQues.Status = false;
                        haveSendQues.Protected = false;
                        _haveSendQuesService.Add(haveSendQues);
                        _haveSendQuesService.Save();
                        break;
                    }
                }
                //
            }

        }
        protected List<ApplicationUser> getNormalUserBasedOnType(Post post)
        {

            List<ApplicationUser> ListAppUser = new List<ApplicationUser>();
            //Khong gui lai 2 lan cho 1 expert

            //Check expert info 
            //First condition : 
            List<SubType> detailUser = _subType.GetAll().Where(x => x.Id_Type == post.Id_Type).ToList();
            if (detailUser == null)
            {

                return null;
            }
            //
            foreach (var item in detailUser)
            {
                ListAppUser.Add(_service.GetUserById(item.Id_User));
            }
            //
            return ListAppUser;
        }
        protected List<ApplicationUser> getExpertUserBasedOnType(Post post)
        {
            //
            if (post.Option == 0)
                return null;
            List<ApplicationUser> ListAppUser = new List<ApplicationUser>();
            //Khong gui lai 2 lan cho 1 expert

            //Check expert info 
            //First condition : 
            List<DetailUserType> detailUser = _detailUserTypeService.GetAll().Where(x => x.Type == post.Id_Type).ToList();
            if (detailUser == null)
            {

                return null;
            }
            //
            foreach (var item in detailUser)
            {
                ListAppUser.Add(_service.GetUserById(item.UserID));
            }
            //
            return ListAppUser;
        }
        protected async System.Threading.Tasks.Task<string> sendNofityToMessenger(Post post, ApplicationUser user)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + user.Id_Messenger + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a294932e4b0d0e603f29776";
            paramChatfuel += "&title=Bạn có 1 câu hỏi :" + post.Content;
            //MaHoaMD5.Encrypt()
            paramChatfuel += "&url=http://localhost:20000/Post?id=" + post.Id;
            paramChatfuel += "&idques=" + post.Id;
            paramChatfuel += ChatBotMessenger.getVocaNull();
            var response2 = await client.PostAsync(paramChatfuel, null);
            return "";
        }
        [AllowAnonymous]
        public string busyNotReplay(string idques, string idmessenger)
        {

            ChatfuelJson result = new ChatfuelJson();
            MessJson messPron = new MessJson();

            //
            bool isHetGio = false;
            int idQuestion = int.Parse(idques);
            var currentPost = _postService.GetById(idQuestion);
            var userId = _service.listUserID().Where(x => x.Id_Messenger == idmessenger).FirstOrDefault().Id;
            //Kiem tra xem con thoi gian tra loi hay ko
            var sendQues = _haveSendQuesService.GetAll().Where(x => x.QuesID == idQuestion && x.UserID == userId).ToList().FirstOrDefault();
            if (sendQues != null && currentPost.Post_Status != 10)
            {
                var timePost = sendQues.CreatedDate.Value.AddMinutes(TimeSetting.LimitMinuteForPost()).Ticks;
                var timeCurrent = DateTime.Now.Ticks;
                if (timePost < timeCurrent)
                {
                    isHetGio = true;
                }

            }
            else
            {
                messPron.text = "Câu hỏi không tồn tại hoặc đã trả lời";
                result.messages.Add(messPron);
                return JsonConvert.SerializeObject(result);
            }
            if (!isHetGio)
            {
                messPron.text = "Bạn đã bỏ qua câu hỏi số :" + idques;
                currentPost.CreatedDate = DateTime.Now.AddMinutes(-TimeSetting.LimitMinuteForPost());
                _postService.Update(currentPost);
                _postService.Save();
                result.messages.Add(messPron);
                return JsonConvert.SerializeObject(result);
            }
            messPron.text = "Hết giờ hoặc đã trả lời";
            result.messages.Add(messPron);
            return JsonConvert.SerializeObject(result);
        }
        [AllowAnonymous]
        public async Task<string> replayDirectly(string idques, string ketqua, string idmessenger)
        {
            //
            ChatfuelJson result = new ChatfuelJson();
            MessJson messPron = new MessJson();
            //
            bool checkGio = false;
            int idQuestion = int.Parse(idques);
            var currentPost = _postService.GetById(idQuestion);
            var userId = _service.listUserID().Where(x => x.Id_Messenger == idmessenger).FirstOrDefault().Id;
            //Kiem tra xem con thoi gian tra loi hay ko
            var sendQues = _haveSendQuesService.GetAll().Where(x => x.QuesID == idQuestion && x.UserID == userId).ToList().FirstOrDefault();
            if (sendQues != null)
            {
                int tempMinute = TimeSetting.LimitMinuteForPost();
                var timePost = sendQues.CreatedDate.Value.AddMinutes(tempMinute).Ticks;
                var timeCurrent = DateTime.Now.AddMinutes(-5).Ticks;
                if (timePost < timeCurrent)
                {
                    checkGio = true;
                }
                else
                {
                    currentPost.Post_Status = 10;
                    Comment comment = new Comment();
                    comment.Corrected = false;
                    comment.Content = ketqua;
                    comment.Id_User = userId;
                    comment.Id_Post = idQuestion;
                    comment.DateComment = DateTime.Now.Ticks.ToString();
                    _commentOfPost.Add(comment);
                    _commentOfPost.Save();
                    _postService.Update(currentPost);
                    _postService.Save();
                    messPron.text = "Cám ơn bạn đã phản hồi";
                    result.messages.Add(messPron);
                    await notifyForUserAboutQues(idQuestion);
                    return JsonConvert.SerializeObject(result);
                }

            }
            //
            //  currentPost.Post_Status = 1;
            // _postService.Update(currentPost);
            //_postService.Save();
            // DateTime 
            messPron.text = "Hết hạn trả lời, cảm ơn bạn đã quan tâm";
            result.messages.Add(messPron);
            return JsonConvert.SerializeObject(result);

        }
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
        //Thông báo cho các user đăng ký loại câu hỏi đó
        //public async Task<string> notifyForAllUserAboutQues(int idQues)
        //{
        //    //Thông báo cho người đặt câu hỏi
        //    var currentPost = _postService.GetById(idQues);
        //    var detailPost = _commentOfPost.GetAll().Where(x => x.Id_Post == idQues).FirstOrDefault();
        //    var userOfPost = _service.GetUserById(currentPost.Id_User);
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    var paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + userOfPost.Id_Messenger + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a2c0352e4b0d0e6161fc7a1";
        //    paramChatfuel += "&traloicauhoi=" + currentPost.Content;
        //    paramChatfuel += "&dapancauhoi=" + detailPost.Content;
        //    var response2 = await client.PostAsync(paramChatfuel, null);
        //}
        //

        // Hiển thị popup khi tạo acc mới, chọn chủ đề theo dõi
        public PartialViewResult showPopupSurvey()
        {
            var subList = _subType.GetAll().Where(x => x.Id_User == User.Identity.GetUserId()).ToList();
            if (subList.Count == 0)
            {
                //Show popup to choose 
                var listPostType = _postTypeService.GetAll().ToList();
                return null;
            }
            return null;
        }
        //
        public PartialViewResult loadNewFeeds()
        {
            var subListOfuser = _subType.GetAll().Where(x => x.Id_User == User.Identity.GetUserId()).ToList();
            return null;
        }

        [HttpPost]
        public JsonResult RegisterType(RegisterPostViewModel data)
        {
            string userId = User.Identity.GetUserId();
            if (data.UserID == userId)
            {
                DetailUserType type;
                _detailUserType.removeByUserId(userId);
                foreach (var item in data.TypeList)
                {
                    type = new DetailUserType();
                    type.UserID = data.UserID;
                    type.Type = item;
                    _detailUserType.Add(type);
                }
                try
                {
                    _detailUserType.Save();
                    return Json(new
                    {
                        result = "success"
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
        //make question directly from Messenger
        [AllowAnonymous]
        public async Task<string> ToeicQues(string content, string idmessenger)
        {

            ChatfuelJson result = new ChatfuelJson();
            MessJson messPron = new MessJson();
            MessJson messPron2 = new MessJson();
            var userMakeQues = _service.listUserID().Where(x => x.Id_Messenger == idmessenger).ToList();
            if (userMakeQues.Count != 0)
            {
                Post newPost = new Model.Post();
                newPost.Content = content;
                newPost.DatePost = DateTime.Now.Ticks.ToString();
                newPost.Id_User = userMakeQues.FirstOrDefault().Id;
                newPost.Id_Type = 8; // TOIEC
                //Send lên nhóm fb

                {
                    //post to fb toiec
                    //Gửi cho admin trước khi dc duyệt

                    //
                    //var IdPost = await _fbService.PostingToGroupFB(newPost.Content);
                    //  newPost.Id_PostFB = IdPost.id;
                }
                _postService.Add(newPost);
                _postService.Save();
                await NotifyForAdminToApprove(newPost.Id);
                //Send notify
                List<ApplicationUser> userSubQues = getNormalUserBasedOnType(newPost);
                foreach (var itemUser in userSubQues)
                {
                    await sendNofityToMessenger(newPost, itemUser);
                    HaveSendQuestion haveSendQues = new HaveSendQuestion();
                    haveSendQues.QuesID = newPost.Id;
                    haveSendQues.UserID = itemUser.Id;
                    haveSendQues.Status = false;
                    haveSendQues.Protected = false;
                    _haveSendQuesService.Add(haveSendQues);
                    _haveSendQuesService.Save();
                }
                messPron.text = "Câu hỏi của bạn đã được gửi đi cho mọi người !";
                result.messages.Add(messPron);
                // messPron2.text = "https://www.facebook.com/" + newPost.Id_PostFB;
                //result.messages.Add(messPron2);
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                messPron.text = "Hình như bạn chưa có tài khoản trên Olympus English";
                result.messages.Add(messPron);
                return JsonConvert.SerializeObject(result);
            }
        }
        //make question directly from Messenger
        public async Task<string> IeltsQues(string content, string messengerid)
        {
            ChatfuelJson result = new ChatfuelJson();
            MessJson messPron = new MessJson();

            var userMakeQues = _service.listUserID().Where(x => x.Id_Messenger == messengerid).ToList();
            if (userMakeQues.Count != 0)
            {
                Post newPost = new Model.Post();
                newPost.Content = content;
                newPost.DatePost = DateTime.Now.Ticks.ToString();
                newPost.Id_User = userMakeQues.FirstOrDefault().Id;
                newPost.Id_Type = 9; // TOIEC
                _postService.Add(newPost);
                _postService.Save();
                //Send notify
                List<ApplicationUser> userSubQues = getNormalUserBasedOnType(newPost);
                foreach (var itemUser in userSubQues)
                {
                    await sendNofityToMessenger(newPost, itemUser);
                    HaveSendQuestion haveSendQues = new HaveSendQuestion();
                    haveSendQues.QuesID = newPost.Id;
                    haveSendQues.UserID = itemUser.Id;
                    haveSendQues.Status = false;
                    haveSendQues.Protected = false;
                    _haveSendQuesService.Add(haveSendQues);
                    _haveSendQuesService.Save();
                }
                messPron.text = "Câu hỏi của bạn đã được gửi đi cho mọi người";
                result.messages.Add(messPron);
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                messPron.text = "Hình như bạn chưa có tài khoản trên Olympus English";
                result.messages.Add(messPron);
                return JsonConvert.SerializeObject(result);
            }
        }

        //Lay danh sách bài post trên group fb có đáp án > 4
        [AllowAnonymous]
        public async Task<string> GetPostFB()
        {
            var listPost = await _fbService.GetListFeedTextOfGroup();

            string output = JsonConvert.SerializeObject(listPost);
            return output;
        }

        public async Task<string> NotifyForAdminToApprove(int idQues)
        {
            var currentPost = _postService.GetById(idQues);
            //  var detailPost = _commentOfPost.GetAll().Where(x => x.Id_Post == idQues).LastOrDefault();
            //var userOfPost = _service.GetUserById(currentPost.Id_User);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + "2040670422625565" + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a35ec92e4b01f1986153d63";
            paramChatfuel += "&cauhoicanduyet=" + currentPost.Content;
            paramChatfuel += "&idquesnew=" + idQues;
            var response2 = await client.PostAsync(paramChatfuel, null);
            return "";
        }
        //Approved from Admin, admin xác nhận , câu hỏi dc post lên nhóm fb
        [AllowAnonymous]
        public async Task<string> ApprovedQuestion(string idques)
        {
            ChatfuelJson result = new ChatfuelJson();
            MessJson messPron = new MessJson();
            result.messages.Add(messPron);

            var currentPost = _postService.GetById(int.Parse(idques));
            var IdPost = await _fbService.PostingToGroupFB(currentPost.Content);
            currentPost.Id_PostFB = IdPost.id;
            _postService.Update(currentPost);
            _postService.Save();
            //Notify for user, this question is approved by admin
            await NotifyForUser(currentPost.Id);
            //
            messPron.text = "Thành công";
            return JsonConvert.SerializeObject(result);
        }
        //Thông báo cho người dùng câu hỏi đã được duyệt vào nhóm fb
        public async Task<string> NotifyForUser(int idQues)
        {
            var currentPost = _postService.GetById(idQues);
            //  var detailPost = _commentOfPost.GetAll().Where(x => x.Id_Post == idQues).LastOrDefault();
            var userOfPost = _service.GetUserById(currentPost.Id_User);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + userOfPost.Id_Messenger + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a35f43fe4b01f19863c896a";
            paramChatfuel += "&questioncontent=" + currentPost.Content;
            var response2 = await client.PostAsync(paramChatfuel, null);
            return "";
        }
    }
}

// Post mã 10 là bị bỏ qua or chưa dc trả lờ


