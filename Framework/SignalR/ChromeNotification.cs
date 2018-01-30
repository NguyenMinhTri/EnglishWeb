using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Framework.SignalR
{
    public class ChromeNotification : Hub
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

        //public void sentNotiRequest(string Id_Friend)
        public void sentNotification(string name)
        {
            string sender = Context.User.Identity.Name;
            Clients.Group(name).receiveNotification(name, sender);
        }

        //public void SendChatMessage(string who, string message)
        //{
        //    Clients.Group(Id_Friend).receiveNotiRequest(Id_Friend);
        //}

        public void sentComment(string name, int id_post, int id_comment)
        {
            //var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            Clients.Group(name).receiveComment(id_post, id_comment);
        }
        public static async Task sendNoti(string email, int id_post, int id_comment)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChromeNotification>();
            //  hubContext.Clients.Group(email).receiveMessage(msg);
            hubContext.Clients.Group(email).receiveComment(id_post, id_comment);
        }
        public void sentFriend(string name)
        {
            string sender = Context.User.Identity.Name;
            Clients.Group(name).receiveFriend(name, sender);
        }
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            Groups.Add(Context.ConnectionId, name);
            return base.OnConnected();
        }
    }
}