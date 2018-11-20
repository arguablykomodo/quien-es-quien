using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class Characteristic {
        int _id;
        string _name;
        string _type;
        string _url;

        public Characteristic()
        {
            _id = -1;
        }
        public Characteristic(int id, string name, string type, string url)
        {
            _id = id;
            _name = name;
            _type = type;
            _url = url;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }
        public string Url { get => _url; set => _url = value; }

        static public List<Characteristic> ListCharacteristics() {
            List<Characteristic> characteristics = new List<Characteristic>();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_GetCharacteristics";

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                characteristics.Add(new Characteristic(
                    Convert.ToInt32(reader["ID"]),
                    reader["name"].ToString(),
                    reader["type"].ToString(),
                    reader["url"].ToString()
                ));
            }

            return characteristics;
        }

        static public Characteristic GetCharacteristic(int id)
        {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_GetCharacteristic";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            Characteristic characteristic = new Characteristic(
                Convert.ToInt32(reader["ID"]),
                reader["name"].ToString(),
                reader["type"].ToString(),
                reader["url"].ToString()
            );

            c.Close();
            return characteristic;
        }

        public static void CreateCharacteristic(Characteristic characteristic)
        {
            SqlConnection connection = new DaB().Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_CreateCharacteristic";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", characteristic.Name);
            command.Parameters.AddWithValue("@type", characteristic.Type);
            command.Parameters.AddWithValue("@url", characteristic.Url);
            command.ExecuteNonQuery();
        }

        public static void EditCharacteristic(Characteristic characteristic)
        {
            SqlConnection connection = new DaB().Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_EditCharacteristic";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", characteristic.Id);
            command.Parameters.AddWithValue("@name", characteristic.Name);
            command.Parameters.AddWithValue("@type", characteristic.Type);
            command.Parameters.AddWithValue("@url", characteristic.Url);
            command.ExecuteNonQuery();
        }

        public static void DeleteCharacteristic(int id)
        {
            SqlConnection connection = new DaB().Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_DeleteCharacteristic";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public static List<Characteristic> GetCharacterCharacteristics(Character character)
        {
            List<Characteristic> characteristics = new List<Characteristic>();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_GetCharacterCharacteristics";
            command.Parameters.AddWithValue("@id", character.Id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) characteristics.Add(new Characteristic(
                Convert.ToInt32(reader["ID"]),
                reader["name"].ToString(),
                reader["type"].ToString(),
                reader["url"].ToString()
            ));

            return characteristics;
        }
    }
}