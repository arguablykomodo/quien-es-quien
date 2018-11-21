using quien_es_quien.Models;
using System.Collections.Generic;
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

        public ActionResult ListCharacteristics() {
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            return View();
        }

        public ActionResult EditCharacteristic(int id, string _action) {
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            switch (_action) {
                case "delete":
                    Characteristic.DeleteCharacteristic(id);
                    return RedirectToAction("ListCharacteristics");
                case "edit":
                    return View(Characteristic.GetCharacteristic(id));
                case "create":
                    return View(new Characteristic());
                default:
                    return RedirectToAction("ListCharacteristics");
            }
        }

        public ActionResult SaveCharacteristic(Characteristic characteristic) {
            if (characteristic.Id == -1) {
                Characteristic.CreateCharacteristic(characteristic);
            }
            else {
                Characteristic.EditCharacteristic(characteristic);
            }
            return RedirectToAction("ListCharacteristics");
        }

        public ActionResult ListCharacters() {
            ViewBag.characters = Character.ListCharacters();
            return View();
        }

        public ActionResult EditCharacter(int id, string _action) {
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            switch (_action) {
                case "delete":
                    Character.DeleteCharacter(id);
                    return RedirectToAction("ListCharacters");
                case "edit":
                    return View(Character.GetCharacter(id));
                case "create":
                    return View(new Character());
                default:
                    return RedirectToAction("ListCharacters");
            }
        }

        public ActionResult SaveCharacter(Character character, List<int> hola) {
            if (character.Id == -1) {
                Character.CreateCharacter(character.Name);
            }
            else {
                Character.EditCharacter(character);
            }
            return RedirectToAction("ListCharacters");
        }
    }
}