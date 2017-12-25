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
        //public void SendChatMessage(string who, string message)
        //{
        //    string name = Context.User.Identity.Name;
        //    Clients.Group(who).addChatMessage(name, message);
        //}
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