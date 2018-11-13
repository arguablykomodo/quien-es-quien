using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.MVC;

namespace quien_es_quien.Models {
    public class Character {
        private string name;
        private int id;

        [Required(ErrorMessage = "Nombre inválido.")]
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}