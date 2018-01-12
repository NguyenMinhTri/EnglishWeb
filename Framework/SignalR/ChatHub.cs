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
        public void sentNotiRequest(string name, int code)
        {
            if (code == -1)
            {
                Clients.Group(name).receiveNotiRequest(name);
            }
            else if (code == 0)
            {
                Clients.Group(name).receiveNotiRequestReject(name);
            }
        }

        //public void sentNotiRequest(string Id_Friend)
        //{
        //    Clients.Group(Id_Friend).receiveNotiRequest(Id_Friend);
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