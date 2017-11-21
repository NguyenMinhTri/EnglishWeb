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
                _viewModel.Roles = _service.GetRolesOfUser(user.Id);
        }
    }
}