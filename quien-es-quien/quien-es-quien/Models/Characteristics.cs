using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class Characteristics {
        public string name;
        public int id;

        public Characteristics(string name, int id) {
            this.name = name;
            this.id = id;
        }

        static public List<Characteristics> ListCharacteristics() {
            List<Characteristics> characteristics_list = new List<Characteristics>();
<<<<<<< HEAD
            if (false) {
                string[] names = new string[] { "Skin-color", "Eye-color" };
                for (int i = 0; i < names.Length; i++) {
                    Models.Characteristics c = new Characteristics(names[i], i);
                    characteristics_list.Add(c);
                }
            }
            else {
=======
            if (DaB.use_connection)  {
                SqlConnection c = new DaB().Connect();
                SqlCommand command = c.CreateCommand();
                command.CommandText = "sp_GetCharacteristics";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.NextResult()) {
                    characteristics_list.Add(new Characteristics(reader["characteristic_name"].ToString(), Convert.ToInt32(reader["ID"])));
                }
            } else {
                string[] names = new string[] { "Skin-color", "Eye-color" };
                for (int i = 0; i < names.Length; i++)
                {
                    Models.Characteristics c = new Characteristics(names[i], i);
                    characteristics_list.Add(c);
                }
            }
            return characteristics_list;
        }
    }

}