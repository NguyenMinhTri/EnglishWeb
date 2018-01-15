using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Framework.SignalR
{
    public class ChatHub : Hub
    {
        public void sentNotiRequest(string name, int code, bool reject)
        {
            if (code == -1)
            {
                string sender = Context.User.Identity.Name;
                Clients.Group(name).receiveNotiRequest(name, sender);
            }
            else if (code == 0)
            {
                Clients.Group(name).receiveNotiRequestReject(name, reject);
            }
        }

        public void sentNotification(string name)
        {
            string sender = Context.User.Identity.Name;
            Clients.Group(name).receiveNotification(name, sender);
        }

        public void sentComment(string name, int id_post, int id_comment)
        {
            Clients.Group(name).receiveComment(id_post, id_comment);
        }

        public void LetsChat(string Cl_Name, string Cl_Message)
        {
            Clients.All.NewMessage(Cl_Name, Cl_Message);

        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            Groups.Add(Context.ConnectionId, name);
            return base.OnConnected();
        }
    }
}