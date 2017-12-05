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
    
    public class HomeController : LayoutController
    {
        IClientHomeService _clientHomeService;
        IPostService _postService;
        public HomeController(  ILayoutService layoutService,
            IClientHomeService clientHomeService,
            IPostService postService
            )
            : base(layoutService)
        {
            _clientHomeService = clientHomeService;
            _postService = postService;
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

        public ActionResult Index()
        {
            
            _viewModel = new HomeViewModel();
            CreateLayoutView("Trang chủ");
            var codes = _clientHomeService.GetCodes();
            LayoutViewModel lay = ViewModel;
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
        public PartialViewResult Post(PostViewModel data)
        {
            Post newPost = new Post();
            _viewModel = new PostViewModel();
            //PostViewModel.Question = data.Question;
            //PropertyCopy(newPost, data);
            FieldHelper.CopyNotNullValue(newPost, data);
            _postService.Add(newPost);
            _postService.Save();
            string url = MaHoaMD5.Encrypt(newPost.Id + "#" + newPost.UpdatedDate);
            return PartialView("_Post", PostViewModel);
        }
        //Replay a question
        [HttpGet]
        public PartialViewResult Replay(string hashcode)
        {
            string strPostIdAndTime =  MaHoaMD5.Decrypt(hashcode);
            int idPost =int.Parse( strPostIdAndTime.Split('#').First().ToString());
           // DateTime 
            return PartialView("_Post", PostViewModel);
        }

        //Check time 5' and send this post for other expert
        [AllowAnonymous]
        public void checkTimeOfPost()
        {

            List<Post> listPost = _postService.GetAll().Where(x => (x.UpdatedDate.Value.AddMinutes(5) > DateTime.UtcNow) && 
                                                                x.Status == false).ToList();
            //
            foreach (var post in listPost)
            {
                
            }
        }
        protected ApplicationUser getExpertUserBasedOnType(int questionType)
        {
            ApplicationUser appUser = new ApplicationUser();
            //First condition : 

            //
            return appUser;
        }
    }
}
