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
    public class AdminFriendController : AdminBaseController<Friend, FriendInputAdminViewData>
    {
        IFriendService _friendService;
        public AdminFriendController(IFriendService friendService, ILayoutAdminService layoutService)
            : base(friendService, layoutService)
        {
            _friendService = friendService;
        }
        //
        // GET: /FriendAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<FriendInputAdminViewData> newPatientItems = new AutoForm<FriendInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
