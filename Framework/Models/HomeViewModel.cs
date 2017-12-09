using Framework.Controllers;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{
    public class HomeViewModel : LayoutViewModel, IRef<HomeController>
    {
        public List<PostType> ListPostType { get; set; }
        public List<PostViewModel> ListPost { get; set; }

    }

    public class CommentViewModel : LayoutViewModel, IRef<HomeController>
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Avatar { get; set; }
        public String UserName { get; set; }
        public String Degree { get; set; }
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
        public int Id { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Avatar { get; set; }
        public String UserName { get; set; }
        public String Id_User { get; set; }
        public String Content { get; set; }
        public int Option { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public String DatePost { get; set; }
        public int Id_Type { get; set; }
        public String TypeToString { get; set; }
        public String Degree { get; set; }
        public int Vote { get; set; }
        public int Comment { get; set; }
    }
}