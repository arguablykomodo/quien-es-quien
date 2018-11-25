using quien_es_quien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class CharacterController : Controller {
        public ActionResult List() {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            ViewBag.characters = Character.ListCharacters();
            return View();
        }

        public ActionResult Edit(int id, string _action) {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            switch (_action) {
                case "delete":
                    Character.DeleteCharacter(id);
                    return RedirectToAction("List");
                case "edit":
                    return View(Character.GetCharacter(id));
                case "create":
                    return View(new Character());
                default:
                    return RedirectToAction("List");
            }
        }

        public ActionResult Save(Character character, List<int> characteristics) {
            if (Session["User"] == null || !((User)Session["User"]).Admin) {
                return RedirectToAction("Index", "Dogcheck", new {
                    msg = "No te hagas el vivo bro, no podes entrar aca sin admin"
                });
            }

            if (!ModelState.IsValid) {
                ViewBag.characteristics = Characteristic.ListCharacteristics();
                ViewBag.types = CharacteristicType.ListCharactersisticType();
                return View("Edit", character);
            }

            if (character.Id == -1) {
                Character.CreateCharacter(character.Name);
            }
            else {
                Character.EditCharacter(character);
            }
            character.SetCharacteristics(characteristics);
            return RedirectToAction("List");
        }
    }
}