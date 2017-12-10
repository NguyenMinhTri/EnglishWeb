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
 		IPostCommentDetailService _commentOfPost;
        IPostTypeService _postTypeService;
        IPostVoteDetailService _postVoteDetailService;

        public HomeController(ILayoutService layoutService,
            IClientHomeService clientHomeService,
            IPostService postService,
            IDetailUserTypeService detailUser,
            IHaveSendQuestionService haveSendQuesService,
            IPostTypeService postTypeService,
            IPostCommentDetailService commentOfPost,
            IPostVoteDetailService postVoteDetailService
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

        public ActionResult Index(PostViewModel data)
        {

            _viewModel = new HomeViewModel();
            CreateLayoutView("Trang chủ");
            HomeViewModel.ListPostType = _postTypeService.GetAll().ToList();
            return View(HomeViewModel);
        }

        [HttpPost]
        public PartialViewResult Comment(CommentViewModel data)
        {
            _viewModel = new CommentViewModel();
            PostCommentDetail comment = new PostCommentDetail();
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
            _postService.Add(newPost);
            _postService.Save();
            string url = MaHoaMD5.Encrypt(newPost.Id + "#" + newPost.UpdatedDate);
            //Send notify
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
            //

            try
            {
                ApplicationUser userPost = _service.GetUserById(newPost.Id_User);
                FieldHelper.CopyNotNullValue(PostViewModel, user);
                FieldHelper.CopyNotNullValue(PostViewModel, newPost);
                PostViewModel.TypeToString = _postTypeService.GetById(newPost.Id_Type).Name;
                PostVoteDetail vote = _postVoteDetailService.getVoteByIdUser(newPost.Id_User);
                if (vote != null)
                {
                    PostViewModel.Vote = vote.Vote;
                }
            }
            catch
            {

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
            List<Post> listPost = _postService.GetAll().Where(post => post.Post_Status == 0 ).ToList();
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
        protected List<ApplicationUser> getExpertUserBasedOnType(Post post)
        {
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
            paramChatfuel +="&idques=" + post.Id;
            paramChatfuel += ChatBotMessenger.getVocaNull();
            var response2 = await client.PostAsync(paramChatfuel, null);
            return "";
        }
        [AllowAnonymous]
        public string busyNotReplay(string idques, string idmessenger)
        {
            RootObject2 result = new RootObject2();
            Message3 messPron = new Message3();
           
            //
            bool isHetGio = false;
            int idQuestion = int.Parse(idques);
            var currentPost = _postService.GetById(idQuestion);
            var userId = _service.listUserID().Where(x => x.Id_Messenger == idmessenger).FirstOrDefault().Id;
            //Kiem tra xem con thoi gian tra loi hay ko
            var sendQues = _haveSendQuesService.GetAll().Where(x => x.QuesID == idQuestion && x.UserID == userId ).ToList().FirstOrDefault();
            if (sendQues != null && currentPost.Post_Status !=10 )
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
            RootObject2 result = new RootObject2();
            Message3 messPron = new Message3();
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
                    PostCommentDetail comment = new PostCommentDetail();
                    comment.Content = ketqua;
                    comment.Id_User = userId;
                    comment.Id_Post = idQuestion;
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
            var currentPost = _postService.GetById(idQues);
            var detailPost = _commentOfPost.GetAll().Where(x => x.Id_Post == idQues).FirstOrDefault();
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

// Post mã 10 là bị bỏ qua or chưa dc trả lờ
