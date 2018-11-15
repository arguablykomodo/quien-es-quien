using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.MVC;

namespace quien_es_quien.Models {
    public class Character {
<<<<<<< HEAD
        [System.ComponentModel.DataAnnotations.Required]
        public string name;
        public int id;
=======
        private string name;
        private int id;

        [Required(ErrorMessage = "Nombre inválido.")]
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
>>>>>>> 660b3b1ba6d10156932ecdd4fdc121f2b5973272
    }
}