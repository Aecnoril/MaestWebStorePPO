using MaestWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaestWebStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(StoreEntities storeEnts)
        {
            storeEnts.GetMostBought();
            if (Request.IsAuthenticated)
            {
                User user = (User)Session["User"];
                storeEnts.GetWishlist(user);
            }

            ViewBag.ListType = "Most bought games: ";
            return View("Index", storeEnts);
        }

        public ActionResult ToStore()
        {
            return RedirectToAction("Index", "Store", "");
        }
    }
}