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
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events.Calendar;
using Framework.Model;
using DayPilot.Web.Mvc.Enums;
using Framework.FrameworkContext;

namespace Framework.Controllers
{
    
    public class CalendarController : LayoutController
    {
        IClientCalendarService _clientCalendarService;
        IEventService _eventService;
        public CalendarController(  ILayoutService layoutService,
            IClientCalendarService clientCalendarService,
            IEventService eventService
            )
            : base(layoutService)
        {
            _clientCalendarService = clientCalendarService;
            _eventService = eventService;
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
            bool result = _eventService.IsFreeTime(User.Identity.Name);
            _viewModel = new CalendarViewModel();
            CreateLayoutView("Lịch biểu");
            LayoutViewModel lay = ViewModel;
            return View(ViewModel);
        }
        //
        public ActionResult Backend()
        {
           
            return new Dpc().CallBack(this);
        }
        class Dpc : DayPilotCalendar
        {
            FrameworkDbContext db = new FrameworkDbContext();

            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Auto);
            }
            protected override void OnEventClick(EventClickArgs e)
            {
                var userID = this.Controller.User.Identity.Name;
                int id = int.Parse(e.Id);
                try
                {
                    var toBeResized = (from ev in db.Events where ev.id == id && ev.userID == userID select ev).First();
                    db.Events.Remove(toBeResized);
                    db.SaveChanges();
                }
                catch
                {

                }
                Update();
            }
            protected override void OnEventResize(EventResizeArgs e)
            {
                var userID = this.Controller.User.Identity.Name;
                int id = int.Parse(e.Id);
                var toBeResized = (from ev in db.Events where ev.id == id && ev.userID == userID select ev).First();
                toBeResized.eventstart = e.NewStart;
                toBeResized.eventend = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                var userID = this.Controller.User.Identity.Name;
                //
                int id = int.Parse(e.Id);
                var toBeResized = (from ev in db.Events where ev.id == id && ev.userID == userID select ev).First();
                toBeResized.eventstart = e.NewStart;
                toBeResized.eventend = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                var toBeCreated = new Event { eventstart = e.Start,userID = this.Controller.User.Identity.Name, eventend = e.End, text = (string)e.Data["name"] };
               // toBeCreated.userID = ((AdminEventController)this.Controller).User.Identity.Name;
                db.Events.Add(toBeCreated);
                db.SaveChanges();
                Update();
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }
                var userID = this.Controller.User.Identity.Name;

                Events = from ev in db.Events where ev.userID == userID select ev;

                DataIdField = "id";
                DataTextField = "text";
                DataStartField = "eventstart";
                DataEndField = "eventend";
            }
        }
    }
}
