using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace quien_es_quien.Models {
    public class Character {
        [System.ComponentModel.DataAnnotations.Required]
        string name;
        int id;

        public Character() {
            this.name = "";
            this.id = -1;
        }
        public Character(string name, int id) {
            this.name = name;
            this.id = id;
        }

        [Required(ErrorMessage = "Nombre inválido.")]
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }

        static public List<Character> ListCharacters() {
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

        static public Character GetCharacter(int id) {
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

        static public void CreateCharacter(string name) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_CreateCharacter";
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        static public void EditCharacter(Character character) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_EditCharacter";
            command.Parameters.AddWithValue("@ID", character.id);
            command.Parameters.AddWithValue("@name", character.name);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        static public void DeleteCharacter(int id) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_DeleteCharacter";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }
    }
}