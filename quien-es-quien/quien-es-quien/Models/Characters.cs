using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace quien_es_quien.Models
{
    public class Character
    {
        [System.ComponentModel.DataAnnotations.Required]
        private string _name;
        private int _id;
        private List<int> _characteristics;

        public Character()
        {
            _name = "";
            _id = -1;
        }
        public Character(string name, int id)
        {
            this._name = name;
            this._id = id;
            if (id != -1)
            {
                SqlConnection c = new DaB().Connect();
                SqlCommand command = c.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id",id);
                command.CommandText = "sp_GetCharacterCharacteristics";
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _characteristics.Add(Convert.ToInt32(reader["id"]));
                }

                c.Close();
            }
        }

        [Required(ErrorMessage = "Ingrese un nombre valido")]
        public string Name { get => _name; set => _name = value; }
        public int Id { get => _id; set => _id = value; }
        public List<int> Characteristics { get => _characteristics; set => _characteristics = value; }

        public static List<Character> ListCharacters()
        {
            List<Character> characters = new List<Character>();

            SqlConnection c = new DaB().Connect();
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

        public static void CreateCharacter(string name)
        {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_CreateCharacter";
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        public static void EditCharacter(Character character)
        {
            SqlConnection c = new DaB().Connect();
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
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_DeleteCharacter";
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            c.Close();
        }

        public void SetCharacteristics(List<int> characteristics)
        {
            // First clear characteristics
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_ClearCharacteristics";
            command.Parameters.AddWithValue("@id", _id);
            command.ExecuteNonQuery();

            // Then add new ones
            foreach (int id in characteristics)
            {
                System.Diagnostics.Debug.Print(id.ToString());
                SqlCommand command2 = c.CreateCommand();
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.CommandText = "sp_AddCharacterCharacteristic";
                command2.Parameters.AddWithValue("@characterid", this._id);
                command2.Parameters.AddWithValue("@characteristicid", id);
                command2.ExecuteNonQuery();
            }

            c.Close();
        }
    }
}