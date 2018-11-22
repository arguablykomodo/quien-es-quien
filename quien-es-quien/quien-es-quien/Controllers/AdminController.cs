using quien_es_quien.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class AdminController : Controller {
        public ActionResult Index() {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            return View();
        }
    }
}