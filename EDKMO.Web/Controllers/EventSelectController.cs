using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EDKMO.Web.Controllers
{
    public class EventSelectController : Controller
    {
        IEventTypeService EventTypeService;
        IEventService EventService;
        ITerritories TerritoriesService;
        IUsers UsersService;

        public EventSelectController(IEventTypeService eventTypeService, IEventService eventService, ITerritories territoriesService, IUsers usersService)
        {
            EventTypeService = eventTypeService;
            EventService = eventService;
            TerritoriesService = territoriesService;
            UsersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string d, byte territoryId, string accountId, string reportId, string name)
        {
            if (string.IsNullOrEmpty(d))
                d = DateTime.Now.Date.ToString();

            EventSelectModel model = new EventSelectModel(DateTime.Parse(d));

            ViewBag.EventTypes = await EventTypeService.ListAll();
            model.Users = await UsersService.ListActive();
            model.Territory = await TerritoriesService.Get(territoryId);

            model.Event.TerritoryId = territoryId;
            model.Event.StartDate = DateTime.Parse(d);
            model.Event.EventDuration = new TimeSpan(0, 30, 0);
            model.Event.AccountId = string.IsNullOrEmpty(accountId) ? (Guid?)null : Guid.Parse(accountId);
            model.Event.ReportMoId = string.IsNullOrEmpty(reportId) ? (Guid?)null : Guid.Parse(reportId);
            model.Event.EventName = name;
            model.Events = await EventService.ListByDate(DateTime.Parse(d));

            model.Users.ForEach(user =>
            {
                user.StartWork = user.StartWork.Add(TimeSpan.FromHours(model.Territory.UTCHours));
                user.EndWork = user.EndWork.Add(TimeSpan.FromHours(model.Territory.UTCHours));
            });
            //Подгоним под территорию время каждого сотрудника



            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(EventDTO obj)
        {
            ViewBag.EventTypes = await EventTypeService.ListAll();

            EventSelectModel model = new EventSelectModel(obj.StartDate);


            model.Event = obj;
            model.Users = await UsersService.ListActive();
            model.Territory = await TerritoriesService.Get(obj.TerritoryId);
            model.Users.ForEach(user =>
            {
                user.StartWork = user.StartWork.Add(TimeSpan.FromHours(model.Territory.UTCHours));
                user.EndWork = user.EndWork.Add(TimeSpan.FromHours(model.Territory.UTCHours));
            });

            EventCreateResult result = await EventService.CreateEvent(model.Event);

            model.Error = result.Error;
            if (string.IsNullOrEmpty(model.Error))
            {
                //Добавлено все
                model.IsSuccess = true;
                model.Event.EventDuration = new TimeSpan(0, 30, 0);
                model.Event.RoadAfter = TimeSpan.Zero;
                model.Event.RoadTo = TimeSpan.Zero;
            }

            model.Events = await EventService.ListByDate(obj.StartDate.Date);

            return View(model);
        }
    }
}