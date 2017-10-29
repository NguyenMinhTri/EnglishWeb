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
    
    public class StatisticsController : LayoutController
    {
        IClientStatisticsService _clientStatisticsService;
        public StatisticsController(  ILayoutService layoutService,
            IClientStatisticsService clientStatisticsService
            )
            : base(layoutService)
        {
            _clientStatisticsService = clientStatisticsService;
        }


        StatisticsViewModel ViewModel
        {
            get
            {
                return (StatisticsViewModel)_viewModel;
            }
        }
        public ActionResult Index()
        {
            _viewModel = new StatisticsViewModel();
            CreateLayoutView("Thống kê");
            LayoutViewModel lay = ViewModel;
            return View(ViewModel);
        }
    }
}
