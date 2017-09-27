using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EDKMO.Web.Areas.Manage.Controllers
{
    public class UsersController : Controller
    {
        IUsers UserService;
        ITerritories TerritoryService;

        public UsersController(IUsers userService, ITerritories territoryService)
        {
            UserService = userService;
            TerritoryService = territoryService;
        }

        // GET: Manage/Users
        public ActionResult Index()
        {
            ViewBag.Query = UserService.Select();
            return View();
        }

        public ActionResult UsersSelectPartial()
        {
            ViewBag.Query = UserService.Select();
            return PartialView(Resources.GridPartialPath.Users);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(byte? id)
        {
            ViewBag.territories = await TerritoryService.GetAll();
            UserDTO model = new UserDTO();
            if (id.HasValue)
                model = await UserService.Get(id.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserDTO model)
        {
            await UserService.Update(model);
            return RedirectToAction("Index");
        }
    }
}