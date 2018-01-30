using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using Framework.FrameworkContext;
using Framework.Model;
using Framework.Service.Admin;
using Framework.ViewData.Admin.Input;
using Framework.ViewData.Control;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Framework.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminEventController : AdminBaseController<Event, EventInputAdminViewData>
    {
        IEventService _eventService;
        public AdminEventController(IEventService eventService, ILayoutAdminService layoutService)
            : base(eventService, layoutService)
        {
            _eventService = eventService;
        }
        //
        // GET: /EventAdmin/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<EventInputAdminViewData> newPatientItems = new AutoForm<EventInputAdminViewData>();
            ViewBag.Forms = newPatientItems.GetControls();
            return View();
        }
        class Dpc : DayPilotCalendar
        {
            FrameworkDbContext db = new FrameworkDbContext();

            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Full);
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                var toBeResized = (from ev in db.Events where ev.id == Convert.ToInt32(e.Id) select ev).First();
                toBeResized.eventstart = e.NewStart;
                toBeResized.eventend = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                var toBeResized = (from ev in db.Events where ev.id == Convert.ToInt32(e.Id) select ev).First();
                toBeResized.eventstart = e.NewStart;
                toBeResized.eventend = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                var toBeCreated = new Event { eventstart = e.Start, eventend = e.End, text = (string)e.Data["name"] };
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

                Events = from ev in db.Events select ev;

                DataIdField = "id";
                DataTextField = "text";
                DataStartField = "eventstart";
                DataEndField = "eventend";
            }
        }
    }
}
