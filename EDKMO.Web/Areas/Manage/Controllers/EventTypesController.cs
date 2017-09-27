using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EDKMO.Web.Areas.Manage.Controllers
{
    public class EventTypesController : Controller
    {
        IEventTypeService EventTypeService;
        public EventTypesController(IEventTypeService service)
        {
            EventTypeService = service;
        }

        // GET: Manage/Users
        public ActionResult Index()
        {
            ViewBag.Query = EventTypeService.Select();
            return View();
        }

        public ActionResult EventTypesSelectPartial()
        {
            ViewBag.Query = EventTypeService.Select();
            return PartialView(Resources.GridPartialPath.EventTypes);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(byte? id)
        {
            EventTypeDTO model = new EventTypeDTO();
            if (id.HasValue)
                model = await EventTypeService.Get(id.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EventTypeDTO model)
        {
            await EventTypeService.Update(model);
            return RedirectToAction("Index");
        }
    }
}