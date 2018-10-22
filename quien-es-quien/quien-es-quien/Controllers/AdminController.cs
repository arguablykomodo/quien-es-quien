using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class AdminController : Controller {
        // GET: Home
        public ActionResult Index() {
            return View();
        }

        public ActionResult Characters() {
            return View();
        }

        public ActionResult ListCharacteristics() {
            Models.DaB daB = new Models.DaB();
            var characteristics = Models.Characteristics.ListCharacteristics();
            ViewBag.characteristisc = characteristics;

            return View();
        }
    }
}