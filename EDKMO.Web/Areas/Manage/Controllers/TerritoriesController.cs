using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EDKMO.Web.Areas.Manage.Controllers
{
    public class TerritoriesController : Controller
    {
        BusinessLogic.Interfaces.ITerritories TerritoryService;

        public TerritoriesController(BusinessLogic.Interfaces.ITerritories territoryService)
        {
            TerritoryService = territoryService;
        }

        // GET: Manage/Territories
        public ActionResult Index()
        {
            ViewBag.Query = TerritoryService.Select();
            return View();
        }

        public ActionResult TerritoriesSelectPartial()
        {
            ViewBag.Query = TerritoryService.Select();
            return PartialView(Resources.GridPartialPath.Users);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            await Task.FromResult(0);
            return View();
        }
    }
}