using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using quien_es_quien.Models;

/*
 10.128.8.16
 QEQC01
     */

namespace quien_es_quien.Models {
    public class DaB {
        public static string connectionString = @"Server=10.128.8.16;User id=QEQC01;Password=QEQC01;Database=QEQC01;Trusted_Connection=true";
        public SqlConnection sql;
        public DaB() {
            Connect();
        }
        ~DaB() {
            Disconnect();
        }

        private SqlConnection Connect() {
            sql = new SqlConnection(connectionString);
            sql.Open();
            return sql;
        }

        public void Disconnect() {
            sql.Close();
        }

        public void UpdateBitcoins(User u, long bitcoins) {
            if(u.Bitcoins - bitcoins < bitcoins && bitcoins < 0) {
                bitcoins = u.Bitcoins;
            }

            SqlConnection connection = Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_UpdateBitcoins";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("username", u.Username);
            command.Parameters.AddWithValue("bitcoins", bitcoins);
            try {
                command.ExecuteNonQuery();
            } catch(Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message);
            }

            u.Bitcoins = u.Bitcoins + bitcoins;
        }

        public User LoginUser(string username, string password) {
            password = Utils.CreateMD5(password);

            SqlConnection connection = Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_Login";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);

            try {
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read()) {
                    int code = Convert.ToInt32(reader["code"]);
                    if(code == 1) {
                        String uname = reader["username"].ToString();
                        long bitcoins = Convert.ToInt64(reader["bitcoins"]);
                        int bestscore = Convert.ToInt32(reader["bestscore"]);
                        return new User(bitcoins, uname, 0, bestscore);
                    }
                }
                return null;
            } catch(Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message);
                return null;
            }
        }

        //As we can't access the DaB we will have to use this override to actually return anything and test the rest of the code
        public List<Characteristics> ListCharacteristics() {
            List<Characteristics> characteristics_list = new List<Characteristics>();
            string[] names = new string[] { "Skin-color", "Eye-color" };
            for(int i = 0; i < 4; i++) {
                Models.Characteristics c;
                c.id = i;
                c.name = names[i];
                characteristics_list.Add(c);
            }
            return characteristics_list;
        }
    }
}