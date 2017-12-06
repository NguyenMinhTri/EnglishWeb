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

namespace Framework.Controllers
{
    
    public class HomeController : LayoutController
    {
        IClientHomeService _clientHomeService;
        IPostService _postService;
        IDetailUserTypeService _detailUserTypeService;
        IHaveSendQuestionService _haveSendQuesService;
        public HomeController(  ILayoutService layoutService,
            IClientHomeService clientHomeService,
            IPostService postService,
            IDetailUserTypeService detailUser,
            IHaveSendQuestionService haveSendQuesService
            )
            : base(layoutService)
        {
            _clientHomeService = clientHomeService;
            _postService = postService;
            _detailUserTypeService = detailUser;
            _haveSendQuesService = haveSendQuesService;
        }


        HomeViewModel ViewModel
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

        public async System.Threading.Tasks.Task<ActionResult> Index(PostViewModel data)
        {

            _viewModel = new HomeViewModel();
            CreateLayoutView("Trang chủ");
            var codes = _clientHomeService.GetCodes();
            LayoutViewModel lay = ViewModel;
            //Temp send messenger
            //Post newPost = new Post();
            ////PostViewModel.Question = data.Question;
            ////PropertyCopy(newPost, data);
            //newPost.Id_User = "4ffdcb92-b150-4ee0-8af1-19ec9d72bf6f";
            //newPost.Content = "Ban co 1 question";
            //newPost.Protected = false;
            //newPost.Status = false;
            //newPost.Id_Type = 3;
            //_postService.Add(newPost);
            //_postService.Save();
            //string url = MaHoaMD5.Encrypt(newPost.Id + "#" + newPost.UpdatedDate);
            //Send notify
          //  ApplicationUser user = getExpertUserBasedOnType(newPost);
            //await sendNofityToMessenger(newPost, user);
            //
            return View(ViewModel);
        }

        [HttpPost]
        public PartialViewResult Comment(CommentViewModel data)
        {
            _viewModel = new CommentViewModel();
            //CommentViewModel.Comment = comment;
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
            _viewModel = new PostViewModel();

            //PostViewModel.Question = data.Question;
            //PropertyCopy(newPost, data);
            FieldHelper.CopyNotNullValue(newPost, data);
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
            return PartialView("_Post", PostViewModel);
        }
        //Replay a question
        [HttpGet]
        public PartialViewResult Replay(string hashcode)
        {
            var currentPost = _postService.GetById(int.Parse(hashcode));
            currentPost.Status = true;
            _postService.Update(currentPost);
            _postService.Save();
           // DateTime 
            return PartialView("_Post", PostViewModel);
        }

        //Check time 5' and send this post for other expert
        [AllowAnonymous]
        public async Task checkTimeOfPost()
        {
                List<Post> listPost = _postService.GetAll().Where(x => (x.CreatedDate.Value.AddMinutes(5) > DateTime.UtcNow) 
                                                                 && x.Status == false  ).ToList();
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
                            post.Status = true; // have send
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
            if(detailUser == null)
            {
                
                return null;
            }
            //
            foreach(var item in detailUser)
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
            var paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + user.Id_Messenger + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=59a43f64e4b03a25b73c0f23";
            paramChatfuel += "&title=" + post.Content;
            //MaHoaMD5.Encrypt()
            paramChatfuel += "&url=http://localhost:20000/Home/Replay?hashcode=" + post.Id;
            paramChatfuel += ChatBotMessenger.getVocaNull();
            var response2 = await client.PostAsync(paramChatfuel, null);
            return "";
        }
    }
}
