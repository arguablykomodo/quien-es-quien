using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quien_es_quien.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(bool err = false)
        {
            ViewBag.err = err;
            return View();
        }

        public ActionResult Login(string name,string pass) {
            Models.DaB daB = new Models.DaB();
            var loggedUser = Models.User.LoginUser(name, pass);
            if(loggedUser==null) {
                Session["User"] = null;
                return RedirectToAction("Index","Login", new { err = true });
            } else {
                Session["User"] = loggedUser;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Register() => View();
        public ActionResult RegisterUser(string user,string pass) {
            var dab = new Models.DaB();
            Models.User.RegisterUser(user, pass, true);

            return RedirectToAction("Index");
        }

        public ActionResult Logout() {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}