using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Framework.Model.Bot;
using Framework.Controllers;
using Framework.Service;
using System.IO;
using Framework.Model;
using System.Web.Mvc;

namespace MessengerBot.Controllers
{
    public class WebhookController : LayoutController
    {

        public WebhookController(ILayoutService layoutService) : base(layoutService)
        {
        }

        [System.Web.Http.AllowAnonymous]
        public ActionResult ReceivePost(BotRequest data)
        {

            return null;
        }
    }
}
