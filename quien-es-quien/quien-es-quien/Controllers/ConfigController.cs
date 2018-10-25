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

        public ActionResult EditCharacteristic(int _id = -1, string name = "", string _action = "se recibió null, cortate la chota")
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
                    System.Diagnostics.Debug.WriteLine("Removed dick");
                    return RedirectToAction("ListCharacteristics");
            }
            System.Diagnostics.Debug.WriteLine("Sorete y la reconcha de tu madre la re puta que te pario");
            throw new System.Exception("La concha de tu madre all boys (si se recibió null me corto la chota: " + _action+" )");
        }
    }
}