using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class Character {
        [System.ComponentModel.DataAnnotations.Required]
        public string name;
        public int id;
    }
}