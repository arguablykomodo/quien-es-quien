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

        public ActionResult EditCharacteristic(int _id = -1, string name = "", string _action = "")
        {
            switch(_action) {
                case "create":
                    ViewBag.action = _action;
                    ViewBag.name = "";
                    ViewBag.id = -1;
                    return View();
                case "edit":
                    ViewBag.action = _action;
                    ViewBag.name = name;
                    ViewBag.id = _id;
                    return View();
                case "post":
                    if(_id == -1) {
                        Models.Characteristic.CreateCharacteristic(name);
                    } else {
                        Models.Characteristic.EditCharacteristic(_id, name);
                    }
                    return RedirectToAction("ListCharacteristics");
                case "delete":
                    Models.Characteristic.DeleteCharacteristic(_id);
                    return RedirectToAction("ListCharacteristics");
            }
            throw new System.Exception("Invalid action \""+_action+"\"");
        }
    }
}