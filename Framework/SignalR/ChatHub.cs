using Framework.Controllers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Framework.SignalR
{
    public class NotificationHub : Hub
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
        public static List<string> Users = new List<string>();

        //public void SendChatMessage(string who, string message)
        //{
        //    Clients.Group(Id_Friend).receiveNotiRequest(Id_Friend);
        //}

        public void sentComment(string name, int id_post, int id_comment)
        {
            Clients.Group(name).receiveComment(id_post, id_comment);
        }

        public void LetsChat(string Cl_Name, string Cl_Message)
        {
            Clients.All.NewMessage(Cl_Name, Cl_Message);

        }
        public static async Task sendNoti(string email, string msg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            //  hubContext.Clients.Group(email).receiveMessage(msg);
            hubContext.Clients.Group(email).receiveMessage(msg);
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
        
        public override Task OnConnected()
        {
            try
            {
                string name = Context.User.Identity.Name;
                if (Users.IndexOf(name) == -1)
                {
                    Users.Add(name);
                }
                // Send the current count of users
                Send(Users.Count);
                Groups.Add(Context.ConnectionId, name);
                //Call other controller
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.PostAsync("http://olympusenglish.azurewebsites.net/Learning/remindVocaOnChrome?email=" + name, null);
            }
            catch
            {

            }
            return base.OnConnected();
        }
        
        public override System.Threading.Tasks.Task OnReconnected()
        {
            try
            {
                string name = Context.User.Identity.Name;
                if (Users.IndexOf(name) == -1)
                {
                    Users.Add(name);
                }
                // Send the current count of users
                Send(Users.Count);
            }
            catch
            {

            }
            return base.OnReconnected();
        }
        
        public override Task OnDisconnected(bool stopCalled )
        {
            try
            {
                if (stopCalled == false)
                {
                    string name = Context.User.Identity.Name;

                    if (Users.IndexOf(name) > -1)
                    {
                        Users.Remove(name);
                    }

                    // Send the current count of users
                    Send(Users.Count);
                }

            }
            catch
            {

            }
            return base.OnDisconnected(stopCalled);
        }
        public void Send(int count)
        {
            // Call the addNewMessageToPage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.updateUsersOnlineCount(count);
        }
    }
}