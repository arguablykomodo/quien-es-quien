using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using quien_es_quien.Models;

namespace quien_es_quien.Controllers {
    public class GameController : Controller {
        public ActionResult Index() {
            ViewBag.characters = Character.ListCharactersDeep();
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            return View();
        }
    }
}