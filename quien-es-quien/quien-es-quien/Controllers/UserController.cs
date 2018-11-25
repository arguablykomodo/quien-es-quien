using quien_es_quien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class UserController : Controller {
        public ActionResult List() {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            } 

            ViewBag.users = quien_es_quien.Models.User.ListUsers();
            return View();
        }

        public ActionResult Edit(int id, string _action) {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }
            
            switch (_action) {
                case "delete":
                    quien_es_quien.Models.User.DeleteUser(id);
                    return RedirectToAction("List");
                case "edit":
                    return View(quien_es_quien.Models.User.GetUser(id));
                default:
                    return RedirectToAction("List");
            }
        }

        public ActionResult Save(User user) {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            if (!ModelState.IsValid) return View("Edit", user);

            quien_es_quien.Models.User.SaveUser(user);
            return RedirectToAction("List");
        }
    }
}