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
        public string Comment{ get; set; }
    }
}