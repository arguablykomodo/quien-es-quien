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
        public static bool use_connection = false;
        public DaB() {
            try {
                Connect();
            } catch {
                use_connection = false;
            }
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
            if(use_connection)
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
            if (!use_connection) {
                if(username=="Comunism"&&password=="DidntFail") {
                    return new User(10, username, 10, 10);
                } else {
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

        
    }
}