using MaestWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MaestWebStore.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            else return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            else return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            //This is horribly atrocious, I know. But I had no time to change what I had into something better.
            if (ModelState.IsValid)
            {
                user = user.IsValid(user);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                    Session["User"] = user;
                    Session["Cart"] = user.CartCount;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        }
        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>MVC Actionresult</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user-model used for registering</param>
        /// <returns>A standard ActionResult</returns>
        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.IsRegistered(user.Username, user.Password))
                {
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Register failed, username already in use");
                }
            }
            return View(user);
        }

        //Only abailable when you're authorized, ofcourse
        [Authorize]
        public ActionResult updateUser()
        {
            User user = (User)Session["User"];
            return View(user);
        }
        /// <summary>
        /// Updates the user-account with newly entered information.
        /// </summary>
        /// <param name="user">The fetched user that is about to be edited</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updateUser(User user)
        {
            if (!user.IsUpdated(user))
            {
                //I would rather use the viewbag, but I'm looking into how that's done exactly. Priority #2 for now.
                ModelState.AddModelError("", "Register failed, username already in use");
            }
            else
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return View(user);
        }
    }
}