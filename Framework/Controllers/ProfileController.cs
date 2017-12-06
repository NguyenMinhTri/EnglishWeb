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

        public ActionResult Index(string option)
        {
            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "newsfeed":
                        {
                            _viewModel = new NewsFeedViewModel();
                            CreateLayoutView("Trang cá nhân");
                            break;
                        }
                    case "about":
                        {
                            _viewModel = new AboutViewModel();
                            CreateLayoutView("Về bạn");
                            break;
                        }
                    case "friend":
                        {
                            _viewModel = new FriendViewModel();
                            CreateLayoutView("Bạn bè");
                            break;
                        }
                    default:
                        {
                            _viewModel = new NewsFeedViewModel();
                            CreateLayoutView("Trang cá nhân");
                            break;
                        }
                }
            }
            else
            {
                _viewModel = new NewsFeedViewModel();
                CreateLayoutView("Trang cá nhân");
            }

            return View(_viewModel);
        }

        [HttpGet]
        public ActionResult NewsFeed()
        {
            _viewModel = new NewsFeedViewModel();
            CreateLayoutView("Trang cá nhân");
            return PartialView("_NewsFeed", NewsFeedViewModel);
        }

        [HttpGet]
        public ActionResult About()
        {
            _viewModel = new AboutViewModel();
            CreateLayoutView("Về bạn");
            FieldHelper.CopyNotNullValue(AboutViewModel, _viewModel.User);
            return PartialView("_About", AboutViewModel);
        }

        [HttpGet]
        public ActionResult Friend()
        {
            _viewModel = new FriendViewModel();
            return PartialView("_Friend", FriendViewModel);
        }
    }
}
