using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace quien_es_quien.Models {
    public class CharacteristicType {
        int _id;
        string _name;

        public CharacteristicType(int id, string name) {
            _id = id;
            _name = name;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        static public List<CharacteristicType> ListCharactersisticType() {
            List<CharacteristicType> charactersisticTypes = new List<CharacteristicType>();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "sp_GetCharacteristicTypes";
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                charactersisticTypes.Add(new CharacteristicType(
                    Convert.ToInt32(reader["id"]),
                    reader["name"].ToString()
                ));
            }

            c.Close();
            return charactersisticTypes;
        }
    }
}