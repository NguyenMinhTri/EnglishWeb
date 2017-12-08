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

namespace Framework.Controllers
{
    
    public class ProfileController : LayoutController
    {
        IClientProfileService _clientProfileService;
        public ProfileController(  ILayoutService layoutService,
            IClientProfileService clientProfileService
            )
            : base(layoutService)
        {
            _clientProfileService = clientProfileService;
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
            FieldHelper.CopyNotNullValue((HeaderViewModel)_viewModel, user);
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
            var user = _service.GetUserByUserName(username);
            FieldHelper.CopyNotNullValue(NewsFeedViewModel, user);
            return PartialView("_NewsFeed", NewsFeedViewModel);
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
