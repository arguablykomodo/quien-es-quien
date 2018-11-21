using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class Character {
        [System.ComponentModel.DataAnnotations.Required]
        private string name;
        private int id;

        public Character() {
            name = "";
            id = -1;
        }
        public Character(string name, int id) {
            this.name = name;
            this.id = id;
        }

        [Required(ErrorMessage = "Nombre inválido.")]
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }

        public static List<Character> ListCharacters() {
            List<Character> characters = new List<Character>();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_ListCharacters";
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                characters.Add(new Character(
                    reader["name"].ToString(),
                    Convert.ToInt32(reader["ID"])
                ));
            }

            c.Close();
            return characters;
        }

        public static Character GetCharacter(int id) {
            SqlConnection c = new DaB().Connect();
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
            return character;
        }

        public static void CreateCharacter(string name) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_CreateCharacter";
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        public static void EditCharacter(Character character) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_EditCharacter";
            command.Parameters.AddWithValue("@ID", character.id);
            command.Parameters.AddWithValue("@name", character.name);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        public static void DeleteCharacter(int id) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_DeleteCharacter";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        public void SetCharacteristics(List<int> characteristics) {
            // First clear characteristics
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_ClearCharacteristics";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            
            // Then add new ones
            foreach (int id in characteristics) {
                System.Diagnostics.Debug.Print(id.ToString());
                SqlCommand command2 = c.CreateCommand();
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.CommandText = "sp_AddCharacterCharacteristic";
                command2.Parameters.AddWithValue("@characterid", this.id);
                command2.Parameters.AddWithValue("@characteristicid", id);
                command2.ExecuteNonQuery();
            }

            c.Close();
        }
    }
}