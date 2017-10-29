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
    }
}