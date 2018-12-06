using quien_es_quien.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class GameController : Controller {
        public ActionResult Index() {
            List<Character> RemainingCharacters = Character.ListCharactersDeep();
            Session["RemainingCharacters"] = RemainingCharacters;
            Session["QuestionsAsked"] = new List<int>();

            if (Session["User"] == null) {
                Session["Bitcoins"] = (long)6000; // 6 questions
                Session["OriginalBitcoins"] = (long)6000;
            }
            else {
                Session["Bitcoins"] = ((User)Session["User"]).Bitcoins;
                Session["OriginalBitcoins"] = ((User)Session["User"]).Bitcoins;
            }


            System.Random random = new System.Random();
            int i = random.Next();
            int charactersCount = RemainingCharacters.Count;
            while (i >= charactersCount) {
                i = random.Next();
            }

            Session["SecretCharacter"] = RemainingCharacters[i];
            Debug.Print("Secret character: " + RemainingCharacters[i].Id);

            ViewBag.characters = Session["RemainingCharacters"];
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            ViewBag.bitcoins = Session["Bitcoins"];

            return View("Play");
        }

        public ActionResult Play(int type = -1, int characteristic = -1) {
            List<Character> RemainingCharacters = Session["RemainingCharacters"] as List<Character>;
            List<Character> newRemainingCharacters = new List<Character>();
            Character SecretCharacter = Session["SecretCharacter"] as Character;

            Session["Bitcoins"] = ((long)Session["Bitcoins"]) - 1000;
            if (Session["User"] != null) {
                ((User)Session["User"]).UpdateBitcoins(-1000);

                if (((User)Session["User"]).Bitcoins <= 0) {
                    return RedirectToAction("Lose");
                }
            }
            else {

                if (((long)Session["Bitcoins"]) <= 0) {
                    return RedirectToAction("Lose");
                }
            }

            for (int i = 0; i < SecretCharacter.Characteristics.Count; i++) {
                if (SecretCharacter.Characteristics[i].Type == type) {
                    bool hasIt = SecretCharacter.Characteristics[i].Id == characteristic;
                    ViewBag.hasIt = hasIt;
                    foreach (Character character in RemainingCharacters) {
                        if ((character.Characteristics[i].Id == characteristic) == hasIt) {
                            newRemainingCharacters.Add(character);
                        }
                    }
                    break;
                }
            }


            if (newRemainingCharacters.Count == 1) {
                return RedirectToAction("Win");
            }

            if (type != -1 && characteristic != -1)
                Session["RemainingCharacters"] = newRemainingCharacters;
            ((List<int>)Session["QuestionsAsked"]).Add(characteristic);

            ViewBag.characters = Session["RemainingCharacters"];
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();
            ViewBag.bitcoins = Session["Bitcoins"];

            return View();
        }

        public ActionResult Guess(int id) {
            Character SecretCharacter = Session["SecretCharacter"] as Character;

            if (SecretCharacter.Id == id) {
                return RedirectToAction("Win", new { bonus = true });
            }
            else {
                if (Session["User"] != null) {
                    Session["Bitcoins"] = (long)Session["Bitcoins"] - 3000;
                    ((User)Session["User"]).UpdateBitcoins(-3000);
                }
                return RedirectToAction("Lose");
            }
        }

        public ActionResult Win(bool bonus = false) {
            if (Session["SecretCharacter"] == null)
                return RedirectToAction("Index", "Dogcheck", new { msg = "No te me hagas el vivo loco, no podes ganar sin jugar primero" });

            long BitcoinsRemaining;
            if (Session["User"] != null) {
                double multiplier;
                if (bonus)
                    multiplier = 2;
                else
                    multiplier = 1.2;

                long extra = (long)System.Math.Floor(((long)Session["OriginalBitcoins"] - (long)Session["Bitcoins"]) * multiplier);
                ViewBag.current = (long)Session["Bitcoins"] + extra;
                ((User)Session["User"]).UpdateBitcoins(extra);
            }
            else {
                BitcoinsRemaining = (long)Session["Bitcoins"];
            }

            ViewBag.original = Session["OriginalBitcoins"];
            ViewBag.character = Session["SecretCharacter"];

            return View();
        }

        public ActionResult Lose() {
            if (Session["SecretCharacter"] == null)
                return RedirectToAction("Index", "Dogcheck", new { msg = "Ni idea por que queres perder a proposito, pero no podes sin jugar primero" });

            ViewBag.character = Session["SecretCharacter"];
            ViewBag.spent = (long)Session["OriginalBitcoins"] - (long)Session["Bitcoins"];
            return View();
        }
    }
}