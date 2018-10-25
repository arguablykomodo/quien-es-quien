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

                while (reader.Read()) {
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

        public static Characteristic CreateCharacteristic(String s)
        {
            SqlConnection connection = new DaB().Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_CreateCharacteristic";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("name", s);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                Models.Characteristic c = new Characteristic(reader["characteristic_name"].ToString(), Convert.ToInt32(reader["ID"]));
                return c;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message + "\nCharacteristic already exits?");
                return null;
            }
        }

        public static void EditCharacteristic(int id, String name, String newName)
        {
            SqlConnection connection = new DaB().Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_EditCharacteristic";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message + "\nInvalid id?");
            }
        }
    }

}