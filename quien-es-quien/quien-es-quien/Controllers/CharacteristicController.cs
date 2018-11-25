using quien_es_quien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class CharacteristicController : Controller {
        public ActionResult List() {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            ViewBag.types = CharacteristicType.ListCharactersisticType();
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            return View();
        }

        public ActionResult Edit(int id, string _action) {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            ViewBag.types = CharacteristicType.ListCharactersisticType();
            switch (_action) {
                case "delete":
                    Characteristic.DeleteCharacteristic(id);
                    return RedirectToAction("List");
                case "edit":
                    return View(Characteristic.GetCharacteristic(id));
                case "create":
                    return View(new Characteristic());
                default:
                    return RedirectToAction("List");
            }
        }

        public ActionResult Save(Characteristic characteristic) {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            if (!ModelState.IsValid) {
                ViewBag.types = CharacteristicType.ListCharactersisticType();
                return View("Edit", characteristic);
            }

            if (characteristic.Id == -1) {
                Characteristic.CreateCharacteristic(characteristic);
            }
            else {
                Characteristic.EditCharacteristic(characteristic);
            }
            return RedirectToAction("List");
        }
    }
}