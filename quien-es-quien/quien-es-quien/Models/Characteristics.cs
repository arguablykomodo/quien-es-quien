using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class Characteristics {
        public string name;
        public int id;

        static public List<Characteristics> ListCharacteristics() {
            List<Characteristics> characteristics_list = new List<Characteristics>();
            if(!DaB.use_connection) {
                string[] names = new string[] { "Skin-color", "Eye-color" };
                for(int i = 0; i < names.Length; i++) {
                    Models.Characteristics c = new Characteristics();
                    c.id = i;
                    c.name = names[i];
                    characteristics_list.Add(c);
                }
            } else
            {
                
            }
            return characteristics_list;
        }
    }
    //As we can't access the DaB we will have to use this override to actually return anything and test the rest of the code

}