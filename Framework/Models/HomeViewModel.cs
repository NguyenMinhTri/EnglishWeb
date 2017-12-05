using Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{
    public class HomeViewModel : LayoutViewModel, IRef<HomeController>
    {
        public string A { get; set; }
    }

    public class CommentViewModel : LayoutViewModel, IRef<HomeController>
    {
        public String Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Avatar { get; set; }
        public String UserName { get; set; }
        public String Id_User { get; set; }
        public int Id_Post { get; set; }
        public int Id_Comment { get; set; }
        public String Content { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Vote { get; set; }
        public String DateComment { get; set; }

    }

    public class PostViewModel : LayoutViewModel, IRef<HomeController>
    {
        public String Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Avatar { get; set; }
        public String UserName { get; set; }
        public String Id_User { get; set; }
        public String Content { get; set; }
        public String Tag { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Vote { get; set; }
        public String DatePost { get; set; }
        public int Id_Type { get; set; }
    }
}