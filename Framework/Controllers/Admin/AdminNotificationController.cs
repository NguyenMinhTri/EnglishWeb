using Framework.Model;
using Framework.Service.Admin;
using Framework.ViewData.Admin.GetData;
using Framework.ViewData.Admin.Input;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminNotificationController : AdminBaseController<Notification, NotificationInputAdminViewData>
    {
        INotificationService _notificationService;
        public AdminNotificationController(INotificationService notificationService, ILayoutAdminService layoutService)
            : base(notificationService, layoutService)
        {
            _notificationService = notificationService;
        }
        //
        // GET: /NotificationAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<NotificationInputAdminViewData> newPatientItems = new AutoForm<NotificationInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
