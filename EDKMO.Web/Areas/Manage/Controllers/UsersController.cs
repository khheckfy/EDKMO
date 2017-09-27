using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDKMO.Web.Areas.Manage.Controllers
{
    public class UsersController : Controller
    {
        BusinessLogic.Interfaces.IUsers UserService;

        public UsersController(BusinessLogic.Interfaces.IUsers userService)
        {
            UserService = userService;
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
    }
}