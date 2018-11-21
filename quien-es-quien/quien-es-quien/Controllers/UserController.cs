using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class UserController : Controller {
        // GET: User
        public ActionResult List() {
            ViewBag.user_list = quien_es_quien.Models.User.ListUsers();
            return View();
        }
        public ActionResult Edit(int id) {
            if (id == -1) {
                return View("EditForm");
            }

            Models.User user = Models.User.GetUser(id);
            if (user == null) {
                return View("EditForm");
            }

            return View("EditForm", user);
        }
        public ActionResult Save(int id) {
            return RedirectToAction("List");
        }
    }
}