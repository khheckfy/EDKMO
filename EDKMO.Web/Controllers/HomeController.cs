﻿using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.Mvc;
using DevExpress.XtraScheduler;
using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Web.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Linq;

namespace EDKMO.Web.Controllers
{
    public class HomeController : Controller
    {
        IUsers UserService;
        IEventService EventService;
        IEventTypeService EventTypeService;

        public HomeController(IUsers userService, IEventService eventService, IEventTypeService eventTypeService)
        {
            UserService = userService;
            EventService = eventService;
            EventTypeService = eventTypeService;
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

        public ActionResult GetEvent(int id)
        {
            EventDTO obj = EventService.Get(id);
            return PartialView(Resources.GridPartialPath.GetEvent, obj);
        }

        public async Task<ActionResult> EventBlockForm(byte userId, string userName)
        {
            EventBlockFormModel model = new EventBlockFormModel(userId, userName);

            model.EventTypes = await EventTypeService.ListAll();
            model.EventTypes = model.EventTypes.Where(n => n.IsRequiredReport == false).ToList();

            return PartialView(Resources.GridPartialPath.EventBlockForm, model);
        }

        [HttpPost]
        public async  Task<JsonResult> CreateEventBlock(EventBlockFormModel model)
        {
            model.DateStart = model.DateStart.Add(model.TimeFrom);
            model.DateEnd = model.DateEnd.Add(model.TimeTo);
            await EventService.CreateBlockEvent(new EventDTO()
            {
                EndDate = model.DateEnd,
                StartDate = model.DateStart,
                EventTypeId = model.EventTypeId,
                UserId = model.UserId,
                IsMainEvent = true,
                LongDescription = model.LongDesc,
                ShortDescription = model.ShortDesc,
            });
            return Json(0);
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
            else
            {
                var SchedulerObject = await EventService.ScheduleObject();
                var list = SchedulerExtension.GetAppointmentsToUpdate<EventDTO>(SchedulerSettingsHelper.CreateSchedulerSettings(null), SchedulerObject.FetchAppointments, SchedulerObject.Resources);
                if (list != null && list.Length > 0)
                {
                    foreach (EventDTO o in list)
                        await EventService.UpdateEvent(o);
                }
            }

            return PartialView(Resources.GridPartialPath.Scheduller, await EventService.ScheduleObject());
        }

        public async Task<JsonResult> SaveEvent(EventDTO model)
        {
            await EventService.UpdateEvent(model);
            return Json(0);
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

    public static class SchedulerSettingsHelper
    {
        public static SchedulerSettings CreateSchedulerSettings(HtmlHelper htmlHelper)
        {

            SchedulerSettings settings = new SchedulerSettings();

            settings.Name = "scheduler";
            settings.CallbackRouteValues = new { Controller = Resources.Controllers.Home, Action = Resources.GridActions.OverviewPartial };
            settings.EditAppointmentRouteValues = new { Controller = Resources.Controllers.Home, Action = Resources.GridActions.OverviewPartialEditAppointment };
            settings.Start = DateTime.Now;

            settings.Views.DayView.Styles.ScrollAreaHeight = Unit.Pixel(300);
            settings.Views.WeekView.Styles.DateCellBody.Height = Unit.Pixel(250);
            settings.Views.FullWeekView.Styles.ScrollAreaHeight = Unit.Pixel(300);
            settings.Views.WeekView.Enabled = false;
            settings.Views.FullWeekView.Enabled = false;
            settings.Views.TimelineView.Enabled = false;
            settings.Views.WorkWeekView.Enabled = true;
            settings.Width = Unit.Percentage(100);
            settings.Views.DayView.Styles.ScrollAreaHeight = 400;
            settings.Views.WorkWeekView.Styles.ScrollAreaHeight = 400;
            settings.Views.DayView.DayCount = 1;
            settings.GroupType = SchedulerGroupType.Resource;

            settings.OptionsCustomization.AllowAppointmentCreate = UsedAppointmentType.None;
            settings.OptionsCustomization.AllowAppointmentEdit = UsedAppointmentType.None;
            settings.OptionsCustomization.AllowInplaceEditor = UsedAppointmentType.None;

            var wt = new TimeOfDayInterval(TimeSpan.FromHours(9), TimeSpan.FromHours(18));
            settings.Views.DayView.WorkTime = wt;
            settings.Views.DayView.ShowWorkTimeOnly = true;
            settings.Views.DayView.TimeScale = TimeSpan.FromMinutes(30);

            settings.Views.WorkWeekView.ShowWorkTimeOnly = true;
            settings.Views.WorkWeekView.WorkTime = wt;
            settings.Views.WorkWeekView.TimeScale = TimeSpan.FromMinutes(30);

            settings.Storage.Appointments.Assign(SchedulerDemoHelper.DefaultAppointmentStorage);
            settings.Storage.Resources.Assign(SchedulerDemoHelper.DefaultResourceStorage);

            settings.Storage.EnableReminders = false;

            settings.ClientSideEvents.AppointmentDeleting = "OnDeleteSelectedAppointment";
            settings.ClientSideEvents.BeginCallback = "OnBeginCallback";
            settings.ClientSideEvents.Init = "schedulerInit";
            settings.ClientSideEvents.AppointmentDoubleClick = "OnAppointmentDoubleClick";

            IEventService eventServie = DependencyResolver.Current.GetService<IEventService>();
            IUsers userService = DependencyResolver.Current.GetService<IUsers>();

            settings.Views.DayView.SetVerticalAppointmentTemplateContent(c =>
            {
                EventDTO e = eventServie.Get(c.AppointmentViewInfo.Appointment.Id);
                System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(htmlHelper, Resources.GridPartialPath.SchedulerAppView, e);
            });

            settings.ClientSideEvents.EndCallback = "schedulerEndCallback";

            settings.Views.WorkWeekView.SetVerticalAppointmentTemplateContent(c =>
            {
                EventDTO e = eventServie.Get(c.AppointmentViewInfo.Appointment.Id);
                System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(htmlHelper, Resources.GridPartialPath.SchedulerAppView, e);
            });

            settings.OptionsForms.SetAppointmentFormTemplateContent(c =>
            {
                EventDTO e = eventServie.Get(c.Appointment.Id);
                System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(htmlHelper, Resources.GridPartialPath.SchedulerAppEdit, e);
            });

            var users = userService.ListActiveRazor();

            settings.SetHorizontalResourceHeaderTemplateContent(c =>
            {
                System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(htmlHelper, Resources.GridPartialPath.SchedulerResourceHeader, new Tuple<byte, string>((byte)c.Resource.Id, c.Resource.Caption));
            });

            return settings;
        }
    }
}