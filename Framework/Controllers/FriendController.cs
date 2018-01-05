using Framework.Service;
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

namespace Framework.Controllers
{
    public class FriendController : LayoutController
    {
        IClientFriendService _friendService;
        public FriendController(ILayoutService layoutService,
            IClientFriendService friendService
            )
            : base(layoutService)
        {
            _friendService = friendService;
        }

        FriendViewModel ViewModel
        {
            get
            {
                return (FriendViewModel)_viewModel;
            }
        }

        [HttpPost]
        public JsonResult Friend_action(FriendViewModel data)
        {
            if (data.Id_User == User.Identity.GetUserId() && data.Id_Friend != null)
            {
                Friend friend = _friendService.FindRelationship(data.Id_User, data.Id_Friend);

                if (friend != null)
                {
                    if (data.CodeRelationshipId == 0)
                    {
                        _friendService.Delete(friend);
                    }
                    else
                    {
                        friend.CodeRelationshipId = data.CodeRelationshipId;
                        _friendService.Update(friend);
                    }
                }
                else
                {
                    FieldHelper.CopyNotNullValue(friend, data);
                    _friendService.Add(friend);
                }
                try
                {
                    _friendService.Save();
                    return Json(new
                    {
                        result = "success",
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

    }
}
