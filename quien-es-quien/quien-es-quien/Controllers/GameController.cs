﻿using quien_es_quien.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Diagnostics;

namespace quien_es_quien.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Index()
        {
            List<Character> remainingCharacters = Character.ListCharactersDeep();
            Session["remainingCharacters"] = remainingCharacters;
            Session["questionsAsked"] = new List<int>();

            System.Random random = new System.Random();
            int i = random.Next();
            int charactersCount = remainingCharacters.Count;
            while (i > charactersCount) i = random.Next();
            Session["secretCharacter"] = remainingCharacters[i];
            Debug.Print("Secret character: "+ remainingCharacters[i].Id);

            ViewBag.characters = Session["remainingCharacters"];
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();

            return View("Play");
        }

        public ActionResult Play(int type = -1, int characteristic = -1)
        {
            List<Character> remainingCharacters = Session["remainingCharacters"] as List<Character>;
            List<Character> newRemainingCharacters = new List<Character>();
            Character secretCharacter = Session["secretCharacter"] as Character;

            if (Session["User"] != null)
            {
                ((User)Session["User"]).UpdateBitcoins(-1000);
                if(((User)Session["User"]).Bitcoins <= 0)
                {
                    //No more questions left.
                }
            }
            else
            {
                Session["GuestBitcoins"] = ((int)Session["GuestBitcoins"]) - 1000;
                if(((int)Session["GuestBitcoins"]) <= 0){
                    //No more questions left.
                }
            }

            for (int i = 0; i < secretCharacter.Characteristics.Count; i++)
            {
                if (secretCharacter.Characteristics[i].Type == type)
                {
                    bool hasIt = secretCharacter.Characteristics[i].Id == characteristic;
                    foreach(Character character in remainingCharacters)
                    {
                        if((character.Characteristics[i].Id==characteristic) == hasIt)
                        {
                            newRemainingCharacters.Add(character);
                        }
                    }
                    break;
                }
            }

            if(newRemainingCharacters.Count==1)
            {
                return RedirectToAction("Win");
            }

            Session["remainingCharacters"] = newRemainingCharacters;
            ((List<int>)Session["questionsAsked"]).Add(characteristic);

            ViewBag.characters = newRemainingCharacters;
            ViewBag.characteristics = Characteristic.ListCharacteristics();
            ViewBag.types = CharacteristicType.ListCharactersisticType();

            return View();
        }

        public ActionResult Win()
        {


            return View();
        }
    }
}