using Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Framework.Common;
using Framework.ViewModels;
using Framework.Model;

namespace Framework.Controllers
{
    [Authorize]
    public class LayoutController : Controller
    {
        protected LayoutViewModel _viewModel;
        protected ILayoutService _service;
        //
        // GET: /Layout/
        public LayoutController(ILayoutService layoutService
            )
        {
            _service = layoutService;
        }
        public ActionResult _Layout()
        {
            return View();
        }

        public void CreateLayoutView(string title)
        {
            _viewModel.Title = title;
            String controllerName = this.GetType().Name;
            controllerName = controllerName.Substring(0, controllerName.Length - 10);
            _viewModel.ControllerName = controllerName;
            var roles = new List<string>();
            _viewModel.Roles = roles;
            var userId = User.Identity.GetUserId();
            if (userId == null || userId == "")
                return;
            var user = _service.GetUserById(userId);
            if (user != null && !user.Logined)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
                return;
            }
            _viewModel.User = user;
            if (user != null)
            {
                _viewModel.Roles = _service.GetRolesOfUser(user.Id);
                List<String> listFriend = _service.GetAllFriend(user.Id);
                foreach (String friend in listFriend)
                {
                    FriendChatViewModel friendChatViewModel = new FriendChatViewModel();
                    ApplicationUser userT = _service.GetUserById(friend);
                    FieldHelper.CopyNotNullValue(friendChatViewModel, userT);
                    _viewModel.ListFriend.Add(friendChatViewModel);
                }

                bool flag = false;
                List<Friend> listRequest = _service.GetRelationship(User.Identity.GetUserId());
                foreach (Friend friend in listRequest)
                {
                    NotiFriendViewModel notiFriendViewModel = new NotiFriendViewModel();
                    ApplicationUser userT = new ApplicationUser();
                    if (friend != null)
                    {
                        switch (flag)
                        {
                            case false:
                                {
                                    userT = _service.GetUserById(friend.Id_User);
                                    notiFriendViewModel.Flag = false;
                                    break;
                                }
                            case true:
                                {
                                    userT = _service.GetUserById(friend.Id_Friend);
                                    notiFriendViewModel.Flag = true;
                                    break;
                                }
                        }
                        FieldHelper.CopyNotNullValue(notiFriendViewModel, userT);
                        FieldHelper.CopyNotNullValue(notiFriendViewModel, friend);
                        _viewModel.ListRequest.Add(notiFriendViewModel);
                    }
                    else
                    {
                        flag = true;
                    }
                }
                _viewModel.ListRequest.OrderBy(x => x.CreatedDate);
                ViewBag.listRequest = listRequest.Count - 1;

            }
        }

        public PartialViewResult FriendChatSection()
        {
            List<String> listFriend = _service.GetAllFriend(User.Identity.GetUserId());
            FriendChatSectionViewModel friendSection = new FriendChatSectionViewModel();

            foreach (String friend in listFriend)
            {
                FriendChatViewModel friendViewModel = new FriendChatViewModel();
                ApplicationUser user = _service.GetUserById(friend);
                FieldHelper.CopyNotNullValue(friendViewModel, user);
                friendSection.ListFriend.Add(friendViewModel);
            }
            return PartialView("_FriendChatSection", friendSection);
        }
    }
}