using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework.Controllers;

namespace Framework.ViewModels
{
    public class LayoutViewModel : IRef<LayoutController>
    {
        public string Title { get; set; }
        public string ControllerName { get; set; }
        public List<string> Roles { get; set; }
        public ApplicationUser User { get; set; }
        public List<FriendChatViewModel> ListFriend { get; set; }
        public LayoutViewModel()
        {
            ListFriend = new List<FriendChatViewModel>();
            ListRequest = new List<NotiFriendViewModel>();

        }
        public List<NotiFriendViewModel> ListRequest { get; set; }
    }

    public class FriendChatViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Avatar { get; set; }
        public String Degree { get; set; }
    }

    public class FriendChatSectionViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public List<FriendChatViewModel> ListFriend { get; set; }
        public FriendChatSectionViewModel()
        {
            ListFriend = new List<FriendChatViewModel>();
        }
    }

    public class NotiRequestViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public NotiRequestViewModel()
        {
            ListRequest = new List<NotiFriendViewModel>();

        }
        public List<NotiFriendViewModel> ListRequest { get; set; }
    }
}