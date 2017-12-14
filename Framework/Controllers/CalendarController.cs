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
    
    public class CalendarController : LayoutController
    {
        IClientCalendarService _clientCalendarService;
        public CalendarController(  ILayoutService layoutService,
            IClientCalendarService clientCalendarService
            )
            : base(layoutService)
        {
            _clientCalendarService = clientCalendarService;
        }


        CalendarViewModel ViewModel
        {
            get
            {
                return (CalendarViewModel)_viewModel;
            }
        }
        public ActionResult Index()
        {
            _viewModel = new CalendarViewModel();
            CreateLayoutView("Lịch biểu");
            LayoutViewModel lay = ViewModel;
            return View(ViewModel);
        }
    }
}
