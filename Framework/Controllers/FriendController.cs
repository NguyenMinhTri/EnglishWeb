﻿using Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Common;
using System.Threading;
using System.Web.UI;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Framework.ViewModels;
using Microsoft.AspNet.Identity;
using Framework.Model;
using Framework.Service.Admin;

namespace Framework.Controllers
{
    public class FriendController : LayoutController
    {
        IClientFriendService _friendService;
        IApplicationUserService _applicationUserService;

        public FriendController(ILayoutService layoutService,
            IClientFriendService friendService,
            IApplicationUserService applicationUserService
            )
            : base(layoutService)
        {
            _friendService = friendService;
            _applicationUserService = applicationUserService;
        }

        FriendViewModel ViewModel
        {
            get
            {
                return (FriendViewModel)_viewModel;
            }
        }

        FriendSectionViewModel FriendSectionViewModel
        {
            get
            {
                return (FriendSectionViewModel)_viewModel;
            }
        }

        [HttpPost]
        public JsonResult Friend_action(FriendActionViewModel data)
        {
            if (data.Id_User == User.Identity.GetUserId() && data.Id_Friend != null)
            {
                Friend friend = _friendService.FindRelationship(data.Id_User, data.Id_Friend);
                ApplicationUser user1 = _service.GetUserById(data.Id_User);
                ApplicationUser user2 = _service.GetUserById(data.Id_Friend);

                if (friend != null)
                {
                    if (data.CodeRelationshipId == 0)
                    {
                        _friendService.Delete(friend);
                        if (friend.CodeRelationshipId == 1)
                        {
                            if (user1.Friend > 1)
                            {
                                user1.Friend--;
                            }
                            if (user2.Friend > 1)
                            {
                                user2.Friend--;
                            }
                        }
                    }
                    else
                    {
                        friend.CodeRelationshipId = data.CodeRelationshipId;
                        _friendService.Update(friend);
                        user1.Friend++;
                        user2.Friend++;
                    }
                    _applicationUserService.Update(user1);
                    _applicationUserService.Update(user2);
                }
                else
                {
                    friend = new Friend();
                    FieldHelper.CopyNotNullValue(friend, data);
                    _friendService.Add(friend);
                }
                try
                {
                    _friendService.Save();
                    _applicationUserService.Save();
                    return Json(new
                    {
                        result = "success",
                        id = friend.Id
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

        [HttpGet]
        public ActionResult Index(string keyword)
        {
            bool flag = false;
            _viewModel = new FriendSectionViewModel();
            CreateLayoutView("Tìm kiếm bạn bè");
            if (keyword != null)
            {
                List<String> listUser = _applicationUserService.FindFriend(keyword);
                List<Friend> listFriend = _friendService.GetAllFriends(User.Identity.GetUserId());
                foreach (String user in listUser)
                {
                    FriendViewModel friendViewModel = new FriendViewModel();
                    ApplicationUser userT = _service.GetUserById(user);
                    FieldHelper.CopyNotNullValue(friendViewModel, userT);
                    if (flag == false)
                    {
                        foreach (Friend friend in listFriend)
                        {
                            if (friend.Id_Friend == user || friend.Id_User == user)
                            {
                                friendViewModel.FriendDate = friend.UpdatedDate;
                            }
                            flag = true;
                        }
                    }
                    FriendSectionViewModel.ListFriend.Add(friendViewModel);
                }
            }
            return View(FriendSectionViewModel);
        }
    }
}
