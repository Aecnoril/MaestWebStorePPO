using MaestWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MaestWebStore.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Details(Models.Game game, int? id)
        {
            if (id != null)
            {
                game.LoadGameID((int)id);
                return View(game);
            }
            else
            {
                return Content("Game not found :(");
                //TODO: I should make some custom error pages for those extra grades :)
            }
        }

        /// <summary>
        /// Adds a game to a user session's Cart
        /// </summary>
        /// <param name="id">The AppID of the game</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddCart(int id)
        {
            User user = (User)Session["User"]; //Grab the user

            Game game = new Game();
            game.LoadGameID(id);
            user.Cart.Add(game); //Add the game to the cart

            Session["User"] = user; //Put the user back

            Session["Cart"] = user.CartCount;
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Adds a game to a users wishlist.
        /// </summary>
        /// <param name="id">The AppID of the game</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddWishlist(int id)
        {
            User user = (User)Session["User"];
            user.AddWishlist(id);

            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Remove a game from the users' wishlist.
        /// </summary>
        /// <param name="id">The AppID of the game</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult RemoveWishlist(int id)
        {
            User user = (User)Session["User"];
            user.RemoveWishlist(id);

            return RedirectToAction("Details", new { id = id });
        }
    }
}