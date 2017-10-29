using Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{
    public class EntertainmentViewModel : LayoutViewModel, IRef<EntertainmentController>
    {
        public string A { get; set; }
    }

}