using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class Characteristic {
        public string name;
        public int id;

        public Characteristic()
        {
            this.name = "";
            this.id = -1;
        }
        public Characteristic(string name, int id) {
            this.name = name;
            this.id = id;
        }

        static public List<Characteristic> ListCharacteristics() {
            List<Characteristic> characteristics_list = new List<Characteristic>();

            if (DaB.use_connection)  {
                SqlConnection c = new DaB().Connect();
                SqlCommand command = c.CreateCommand();
                command.CommandText = "sp_GetCharacteristics";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.NextResult()) {
                    characteristics_list.Add(new Characteristic(reader["characteristic_name"].ToString(), Convert.ToInt32(reader["ID"])));
                }
            } else {
                string[] names = new string[] { "Skin-color", "Eye-color" };
                for (int i = 0; i < names.Length; i++)
                {
                    Models.Characteristic c = new Characteristic(names[i], i);
                    characteristics_list.Add(c);
                }
            }
            return characteristics_list;
        }
    }

}