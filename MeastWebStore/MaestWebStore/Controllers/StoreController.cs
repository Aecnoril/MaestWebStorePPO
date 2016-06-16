using MaestWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaestWebStore.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store. Loads up the storemodel
        public ActionResult Index(StoreEntities storeEnts)
        {
            storeEnts.GetGames();
            if (Request.IsAuthenticated)
            {
                User user = (User)Session["User"];
                storeEnts.GetWishlist(user);
            }
            ViewBag.ListType = "Games in the store: ";
            return View("Index", storeEnts);
        }

        public ActionResult Cart(StoreEntities storeEnts)
        {
            User user = (User)Session["User"];
            storeEnts.GetCart(user);
            ViewBag.ListType = "Games in your Cart: ";
            return View("Index", storeEnts);
        }

        public ActionResult BuyCart(StoreEntities storeEnts)
        {
            User user = (User)Session["User"];
            //Logic to actually pay for something
            //TODO: Add the cart to owned games
            user.Cart.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult WishList(StoreEntities storeEnts)
        {
            if(Request.IsAuthenticated)
            {
                User user = (User)Session["User"];
                storeEnts.GetWishlist(user);
            }
            ViewBag.ListType = "Games in your Wishlist: ";
            return View("Index", storeEnts);
        }
    }
}