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
    public class AdminMessageController : AdminBaseController<Message, MessageInputAdminViewData>
    {
        IMessageService _messageService;
        public AdminMessageController(IMessageService messageService, ILayoutAdminService layoutService)
            : base(messageService, layoutService)
        {
            _messageService = messageService;
        }
        //
        // GET: /MessageAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<MessageInputAdminViewData> newPatientItems = new AutoForm<MessageInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }

    }
}
