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
    
    public class HomeController : LayoutController
    {
        IClientHomeService _clientHomeService;
        public HomeController(  ILayoutService layoutService,
            IClientHomeService clientHomeService
            )
            : base(layoutService)
        {
            _clientHomeService = clientHomeService;
        }


        HomeViewModel ViewModel
        {
            get
            {
                return (HomeViewModel)_viewModel;
            }
        }
        public ActionResult Index()
        {
            
            _viewModel = new HomeViewModel();
            CreateLayoutView("Trang chủ");
            var codes = _clientHomeService.GetCodes();
            LayoutViewModel lay = ViewModel;
            return View(ViewModel);
        }
    }
}
