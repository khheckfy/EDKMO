using DevExpress.Web.Mvc;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Web.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EDKMO.Web.Controllers
{
    public class HomeController : Controller
    {
        IUsers UserService;
        IEventService EventService;

        public HomeController(IUsers userService, IEventService eventService)
        {
            UserService = userService;
            EventService = eventService;
        }

        // GET: Home
        public async Task<ActionResult> Index()
        {
            HomeModel model = new HomeModel();

            model.SchedulerObject = await EventService.ScheduleObject();

            return View(model);
        }

        public async Task<ActionResult> OverviewPartial()
        {
            return PartialView(Resources.GridPartialPath.Scheduller, await EventService.ScheduleObject());
        }

        public async Task<ActionResult> OverviewPartialEditAppointment(string cmd, string deleteIds)
        {
            if (cmd == "DELETE")
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                int[] arr = js.Deserialize<int[]>(deleteIds);
                foreach (int id in arr)
                    await EventService.Remove(id);
            }

            return PartialView(Resources.GridPartialPath.Scheduller, await EventService.ScheduleObject());
        }
    }

    public class SchedulerDemoHelper
    {
        static MVCxAppointmentStorage defaultAppointmentStorage;
        public static MVCxAppointmentStorage DefaultAppointmentStorage
        {
            get
            {
                if (defaultAppointmentStorage == null)
                    defaultAppointmentStorage = CreateDefaultAppointmentStorage();
                return defaultAppointmentStorage;
            }
        }

        static MVCxAppointmentStorage CreateDefaultAppointmentStorage()
        {
            MVCxAppointmentStorage appointmentStorage = new MVCxAppointmentStorage();
            appointmentStorage.Mappings.AppointmentId = "EventId";
            appointmentStorage.Mappings.Start = "StartDate";
            appointmentStorage.Mappings.End = "EndDate";
            appointmentStorage.Mappings.Subject = "EventName";
            appointmentStorage.Mappings.ResourceId = "UserId";
            appointmentStorage.Mappings.Description = "LongDescription";

            /*
            appointmentStorage.Mappings.AppointmentId = "EventId";
            appointmentStorage.Mappings.Start = "StartDate";
            appointmentStorage.Mappings.End = "EndDate";
            appointmentStorage.Mappings.Subject = "EventName";
            appointmentStorage.Mappings.Description = "ShortDescription";
            appointmentStorage.Mappings.Location = "LongDescription";
            appointmentStorage.Mappings.AllDay = "AllDay";
            appointmentStorage.Mappings.Type = "EventTypeId";
            appointmentStorage.Mappings.RecurrenceInfo = "ShortDescription";
            appointmentStorage.Mappings.ReminderInfo = "ShortDescription";
            appointmentStorage.Mappings.Label = "ShortDescription";
            appointmentStorage.Mappings.Status = "ShortDescription";
            appointmentStorage.Mappings.ResourceId = "UserId";*/
            return appointmentStorage;
        }

        static MVCxResourceStorage defaultResourceStorage;
        public static MVCxResourceStorage DefaultResourceStorage
        {
            get
            {
                if (defaultResourceStorage == null)
                    defaultResourceStorage = CreateDefaultResourceStorage();
                return defaultResourceStorage;
            }
        }
        static MVCxResourceStorage CreateDefaultResourceStorage()
        {
            MVCxResourceStorage resourceStorage = new MVCxResourceStorage();
            resourceStorage.Mappings.ResourceId = "UserId";
            resourceStorage.Mappings.Caption = "LastName";
            return resourceStorage;
        }
    }
}