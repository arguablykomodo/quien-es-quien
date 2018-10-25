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

        public ActionResult EditCharacteristic(int id = -1, string name = "", string action = "")
        {
            switch (action)
            {
                case "create":
                    ViewBag.action = action;
                    return View();
                case "edit":
                    ViewBag.action = action;
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
                    return View("ListCharacteristics");
                case "delete":
                    Models.Characteristic.DeleteCharacteristic(id);
                    return View("ListCharacteristics");
            }

            return View();
        }
    }
}