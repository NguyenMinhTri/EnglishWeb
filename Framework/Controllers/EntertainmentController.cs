using Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Service.Admin;
using Framework.Common;
using System.Threading;
using System.Web.UI;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Framework.ViewModels;
using Framework.Service.Client;

namespace Framework.Controllers
{
    
    public class EntertainmentController : LayoutController
    {
        IClientEntertainmentService _clientEntertainmentService;
        public EntertainmentController(  ILayoutService layoutService)
            : base(layoutService)
        {
        }

        EntertainmentViewModel ViewModel
        {
            get
            {
                return (EntertainmentViewModel)_viewModel;
            }
        }
        public ActionResult Index()
        {
            _viewModel = new EntertainmentViewModel();
            CreateLayoutView("Giải trí");
            LayoutViewModel lay = ViewModel;
            return View(ViewModel);
        }
    }
}
