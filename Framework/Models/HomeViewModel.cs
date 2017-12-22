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
        public List<PostTypeViewModel> ListPostType { get; set; }
        public List<PostViewModel> ListPost { get; set; }
        public HomeViewModel()
        {
            ListPostType = new List<PostTypeViewModel>();
            ListPost = new List<PostViewModel>();
        }
    }

    public class MorePostViewModel : LayoutViewModel, IRef<HomeController>
    {
        public List<PostViewModel> ListPost { get; set; }
        public string UserId { get; set; }
        public string Degree { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public MorePostViewModel()
        {
            ListPost = new List<PostViewModel>();
        }
    }

    public class PostTypeViewModel : LayoutViewModel, IRef<HomeController>
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public bool Register { get; set; }
    }

    public class CommentViewModel : LayoutViewModel, IRef<HomeController>
    {
        public CommentViewModel()
        {
           listChildComment = new List<CommentViewModel>();
        }
        public List<CommentViewModel> listChildComment { get; set; }
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
        public bool Corrected { get; set; }
    }

    public class PostViewModel : LayoutViewModel, IRef<HomeController>
    {
        public PostViewModel()
        {
           listComment = new List<CommentViewModel>();
        }
        public List<CommentViewModel> listComment { get; set; }
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

    public class VoteViewModel : LayoutViewModel, IRef<HomeController>
    {
        public String Id_User { get; set; }
        public int Id_Comment { get; set; }
        public int Id_Post { get; set; }
        public int Vote { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
    }

    public class MarkAnswerViewModel : LayoutViewModel, IRef<HomeController>
    {
        public int Id_Comment { get; set; }
        public int Id_Post { get; set; }
        public String Id_User { get; set; }
        public bool Corrected { get; set; }
    }

    public class RegisterPostViewModel : LayoutViewModel, IRef<HomeController>
    {
        public string UserID { get; set; }
        public List<int> TypeList { get; set; }
        public RegisterPostViewModel()
        {
            TypeList = new List<int>();
        }
    }
}