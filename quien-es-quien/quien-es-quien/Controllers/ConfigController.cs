using System.Web.Mvc;

namespace quien_es_quien.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Characters()
        {
            return View();
        }

        public ActionResult ListCharacteristics()
        {

            Models.DaB daB = new Models.DaB();

            System.Collections.Generic.List<Models.Characteristic> characteristics = Models.Characteristic.ListCharacteristics();
            ViewBag.characteristics = characteristics;

            return View();
        }

        public ActionResult EditCharacteristic(string name = "", int id = -1, string action = "")
        {
            switch (action)
            {
                case "create":
                    ViewBag.action = action;
                    return View();
                case "post":
                    break;
                case "edit":
                    break;
            }

            return View();
        }
    }
}