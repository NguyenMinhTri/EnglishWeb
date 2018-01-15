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
using Framework.Service.Admin;

namespace Framework.Controllers
{
    public class NotificationController : LayoutController
    {
        IClientNotificationService _friendService;
        INotificationService _notificationService;
        IApplicationUserService _applicationUserService;

        public NotificationController(ILayoutService layoutService,
            IClientNotificationService friendService,
            IApplicationUserService applicationUserService,
            INotificationService notificationService
            )
            : base(layoutService)
        {
            _friendService = friendService;
            _applicationUserService = applicationUserService;
            _notificationService = notificationService;
        }

        NotificationViewModel ViewModel
        {
            get
            {
                return (NotificationViewModel)_viewModel;
            }
        }

        [HttpPost]
        public JsonResult Notification_action(string id)
        {
            if (id != null)
            {
                Notification notification = _notificationService.GetById(Convert.ToInt16(id));

                if (notification != null)
                {
                    _notificationService.Delete(notification);
                }
                try
                {
                    _notificationService.Save();
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
