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

            if (Session["User"] == null)
            {
                Session["GuestBitcoins"] = 10000;//10 questions
            } else {
                Session["OriginalBitcoins"] = ((User)Session["User"]).Bitcoins;
            }


            System.Random random = new System.Random();
            int i = random.Next();
            int charactersCount = RemainingCharacters.Count;
            while (i > charactersCount)
            {
                i = random.Next();
            }

            Session["SecretCharacter"] = RemainingCharacters[i];
            Debug.Print("Secret character: " + RemainingCharacters[i].Id);

            ViewBag.characters = Session["RemainingCharacters"];
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();

            return View("Play");
        }

        public ActionResult Play(int type = -1, int characteristic = -1) {
            List<Character> RemainingCharacters = Session["RemainingCharacters"] as List<Character>;
            List<Character> newRemainingCharacters = new List<Character>();
            Character SecretCharacter = Session["SecretCharacter"] as Character;

            if (Session["User"] != null) {
                ((User)Session["User"]).UpdateBitcoins(-1000);

                if (((User)Session["User"]).Bitcoins <= 0) {
                    return RedirectToAction("Lose");
                }
            }
            else {
                Session["GuestBitcoins"] = ((int)Session["GuestBitcoins"]) - 1000;

                if (((int)Session["GuestBitcoins"]) <= 0) {
                    return RedirectToAction("Lose");
                }
            }

            for (int i = 0; i < SecretCharacter.Characteristics.Count; i++) {
                if (SecretCharacter.Characteristics[i].Type == type) {
                    bool hasIt = SecretCharacter.Characteristics[i].Id == characteristic;
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

            if(type!=-1&&characteristic!=-1)
                Session["RemainingCharacters"] = newRemainingCharacters;
            ((List<int>)Session["QuestionsAsked"]).Add(characteristic);

            ViewBag.characters = Session["RemainingCharacters"];
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();

            return View();
        }

        public ActionResult Guess(int id)
        {
            Character SecretCharacter = Session["SecretCharacter"] as Character;

            ((User)Session["User"]).UpdateBitcoins(-3000);

            if(SecretCharacter.Id==id)
            {
                return RedirectToAction("Win",new { bonus=true });
            } else {
                return RedirectToAction("Play");
            }

        }

        public ActionResult Win(bool bonus = false)
        {
            long OriginalBitcoins = (long)Session["OriginalBitcoins"];

            long BitcoinsRemaining;
            if (Session["User"] != null) {
                BitcoinsRemaining = (long)((User)Session["User"]).Bitcoins;

                double multiplier;
                if (bonus)
                    multiplier = 2;
                else
                    multiplier = 1.2;

                ((User)Session["User"]).UpdateBitcoins((long)System.Math.Floor((OriginalBitcoins - BitcoinsRemaining) * multiplier));
            } else {
                BitcoinsRemaining = (long)Session["GuestBitcoins"];
            }


            ViewBag.SpentBitcoins = OriginalBitcoins - BitcoinsRemaining;
            ViewBag.Bitcoins = BitcoinsRemaining;
            ViewBag.SecretCharacter = Session["SecretCharacter"];
            
            return View();
        }

    }
}