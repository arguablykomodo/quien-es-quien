using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quien_es_quien.Controllers {
    public class DogcheckController : Controller {
        public ActionResult Index(string msg) {
            ViewBag.msg = msg;
            return View();
        }
    }
}