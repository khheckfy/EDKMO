using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: Events
        public ActionResult Index(string d)
        {
            EventSelectModel model = new EventSelectModel();

            DateTime date = DateTime.Parse(d).Date;
            while (true)
            {
                date = date.AddMinutes(30);
                if (date.TimeOfDay.Hours < 9 || date.TimeOfDay.Hours > 17)
                    continue;
                if (date.DayOfWeek == DayOfWeek.Saturday)
                    break;
                model.DateTimes.Add(new EventPlaningItem(date));

            }

            return View(model);
        }
    }
}