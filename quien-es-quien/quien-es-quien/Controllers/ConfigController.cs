using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class ConfigController : Controller {
        // GET: Home
        public ActionResult Index() {
            return View();
        }

        public ActionResult Characters() {
            return View();
        }

        public ActionResult ListCharacteristics(Models.Characteristic characteristic = null )
        {
            
            Models.DaB daB = new Models.DaB();

            if (characteristic==null)
            {
                if(characteristic.id==-1)
                {
                    daB.CreateCharacteristic(characteristic.name);
                }
                else
                {
                    
                }
            }

            var characteristics = Models.Characteristic.ListCharacteristics();
            ViewBag.characteristics = characteristics;

            return View();
        }
    }
}