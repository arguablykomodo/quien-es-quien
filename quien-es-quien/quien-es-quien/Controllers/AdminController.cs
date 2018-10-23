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

        public ActionResult ListCharacteristics(string new_characteristic = null)
        {
            Models.DaB daB = new Models.DaB();

            if (new_characteristic!=null)
            {
                daB.CreateCharacteristic(new_characteristic);
            }

            var characteristics = Models.Characteristics.ListCharacteristics();
            ViewBag.characteristics = characteristics;

            return View();
        }
    }
}