using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace quien_es_quien.Models {
    public class Character {
        [System.ComponentModel.DataAnnotations.Required]
        public string name;
        public int id;

        [Required(ErrorMessage = "Nombre inválido.")]
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}