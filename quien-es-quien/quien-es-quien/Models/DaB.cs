using quien_es_quien.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/*
 10.128.8.16
 QEQC01
     */

namespace quien_es_quien.Models {
    public class DaB {
        public static string connectionString = "Server=10.128.8.16;User id=QEQC01;Password=QEQC01;Database=QEQC01;Trusted_Connection=true";
        public SqlConnection sql;
        public static bool use_connection = true;
        public DaB() {
            try {
                Connect();
            }
            catch {
                use_connection = false;
            }
        }
        ~DaB() {
            Disconnect();
        }
        public SqlConnection Connect() {
            sql = new SqlConnection(connectionString);
            sql.Open();
            return sql;
        }

        public void Disconnect() {
            if (use_connection)
                sql.Close();
        }

        public bool UpdateBitcoins(User u, long bitcoins) {
            if (u.Bitcoins - bitcoins < bitcoins && bitcoins < 0) {
                bitcoins = u.Bitcoins;
            }

            SqlConnection connection = Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_UpdateBitcoins";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("username", u.Username);
            command.Parameters.AddWithValue("bitcoins", bitcoins);
            try {
                SqlDataReader reader = command.ExecuteReader();
                return Convert.ToBoolean(reader["code"]);
            }
            catch (Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message + "\nWrong username?");
                return false;
            }

            u.Bitcoins = u.Bitcoins + bitcoins;
        }

        public User LoginUser(string username, string password) {
            if (!use_connection) {
                if (username == "Comunism" && password == "DidntFail") {
                    return new User(10, username, 10, 10);
                }
                else {
                    return null;
                }
            }

            password = Utils.CreateMD5(password);

            SqlConnection connection = Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_Login";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);

            try {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) {
                    int code = Convert.ToInt32(reader["code"]);
                    if (code == 1) {
                        String uname = reader["username"].ToString();
                        long bitcoins = Convert.ToInt64(reader["bitcoins"]);
                        int bestscore = Convert.ToInt32(reader["bestscore"]);
                        return new User(bitcoins, uname, 0, bestscore);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message + "\nWrong credentials?");
                return null;
            }
        }

        static public Characteristics CreateCharacteristic(String s) {
            DaB daB = new DaB();
            SqlConnection connection = daB.Connect();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_CreateCharacteristic";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("name", s);
            try {
                SqlDataReader reader = command.ExecuteReader();
                Models.Characteristics c = new Characteristics(reader["characteristic_name"].ToString(), Convert.ToInt32(reader["ID"]));
                return c;
            }
            catch (Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message + "\nCharacteristic already exits?");
                return null;
            }
        }
    }
}