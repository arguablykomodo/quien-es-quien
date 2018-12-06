using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace quien_es_quien.Models
{
    public class Character
    {
        private string _name;
        private int _id;
        [NonSerialized]
        private List<Characteristic> _characteristics;

        public Character()
        {
            _name = "";
            _id = -1;
            Character robado = Character.GetCharacter((new System.Random()).Next(1, 10));
            _characteristics = robado.Characteristics;
        }

        public Character(string name, int id)
        {
            this._name = name;
            this._id = id;
            _characteristics = new List<Characteristic>();
        }

        public int Id { get => _id; set => _id = value; }
        public List<Characteristic> Characteristics { get => _characteristics; set => _characteristics = value; }
        [Required(ErrorMessage = "Ingrese un nombre valido")]
        public string Name { get => _name; set => _name = value; }

        public static List<Character> ListCharacters()
        {
            List<Character> characters = new List<Character>();

            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_ListCharacters";
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                characters.Add(new Character(
                    reader["name"].ToString(),
                    Convert.ToInt32(reader["ID"])
                ));
            }

            c.Close();
            return characters;
        }

        public static Character GetCharacter(int id)
        {
            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_GetCharacter";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Character character = new Character(
                reader["name"].ToString(),
                Convert.ToInt32(reader["ID"])
            );
            c.Close();

            SqlConnection c2 = Utils.Connect();
            SqlCommand command2 = c2.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.Parameters.AddWithValue("@id", id);
            command2.CommandText = "sp_GetCharacterCharacteristics";
            SqlDataReader reader2 = command2.ExecuteReader();

            while (reader2.Read()) {
                character.Characteristics.Add(new Characteristic(
                    Convert.ToInt32(reader2["ID"]),
                    reader2["name"].ToString(),
                    Convert.ToInt32(reader2["type"]),
                    reader2["url"].ToString()
                ));
            }
            c2.Close();

            return character;
        }

        public static int CreateCharacter(Character character)
        {
            int id = -1;
            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_CreateCharacter";
            command.Parameters.AddWithValue("@name", character.Name);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                id = Convert.ToInt32(reader["ID"]);
            }

            c.Close();
            return id;
        }

        public static void EditCharacter(Character character)
        {
            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_EditCharacter";
            command.Parameters.AddWithValue("@ID", character._id);
            command.Parameters.AddWithValue("@name", character._name);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        public static void DeleteCharacter(int id)
        {
            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_DeleteCharacter";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }
        
        public static List<Character> ListCharactersDeep() {
            List<Character> characters = new List<Character>();
            List<int> ids = new List<int>();

            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_ListCharactersDeep";
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                int i = ids.IndexOf(Convert.ToInt32(reader["ID"]));
                if (i == -1) {
                    ids.Add(Convert.ToInt32(reader["ID"]));
                    characters.Add(new Character(
                        reader["name"].ToString(),
                        Convert.ToInt32(reader["ID"])
                    ));
                    characters[characters.Count - 1].Characteristics.Add(new Characteristic(
                        Convert.ToInt32(reader["cID"]),
                        reader["name"].ToString(),
                        Convert.ToInt32(reader["type"]),
                        reader["url"].ToString()
                    ));
                } else {
                    characters[i].Characteristics.Add(new Characteristic(
                        Convert.ToInt32(reader["cID"]),
                        reader["name"].ToString(),
                        Convert.ToInt32(reader["type"]),
                        reader["url"].ToString()
                    ));
                }
            }

            c.Close();
            return characters;
        }

        public void SetCharacteristics()
        {
            // First clear characteristics
            SqlConnection c = Utils.Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_ClearCharacteristics";
            command.Parameters.AddWithValue("@id", _id);
            command.ExecuteNonQuery();

            // Then add new ones
            foreach (Characteristic ch in _characteristics)
            {
                SqlCommand command2 = c.CreateCommand();
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.CommandText = "sp_AddCharacterCharacteristic";
                command2.Parameters.AddWithValue("@characterid", this._id);
                command2.Parameters.AddWithValue("@characteristicid", ch.Id);
                command2.ExecuteNonQuery();
            }

            c.Close();
        }
    }
}