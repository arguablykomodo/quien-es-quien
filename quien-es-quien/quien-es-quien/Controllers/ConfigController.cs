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

        public ActionResult EditCharacteristic(int id = -1, string name = "", string _action = "")
        {
            switch (_action)
            {
                case "create":
                    ViewBag.action = _action;
                    return View();
                case "edit":
                    ViewBag.action = _action;
                    ViewBag.name = name;
                    ViewBag.id = id;
                    return View();
                case "post":
                    if (id == -1)
                    {
                        Models.Characteristic.CreateCharacteristic(name);
                    }
                    else
                    {
                        Models.Characteristic.EditCharacteristic(id, name);
                    }
                    return RedirectToAction("ListCharacteristics");
                case "delete":
                    Models.Characteristic.DeleteCharacteristic(id);
                    
                    return RedirectToAction("ListCharacteristics");
            }
            System.Diagnostics.Debug.WriteLine("Sorete y la reconcha de tu madre la re puta que te pario");
            throw new System.Exception("La concha de tu madre all boys");
        }
    }
}