using Framework.Common;
using Framework.Model;
using Framework.Service.Admin;
//using Framework.ViewData.BlogContentClient;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.SignalR
{
    public class CommentSignalHub : Hub
    {
        public void Send(String avatar,String name,String content)
        {
            Clients.All.addNewComment(avatar, name, content);
        }
    }
}