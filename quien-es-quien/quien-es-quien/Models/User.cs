using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace quien_es_quien.Models {
    public class User {
        private long _bitcoins;
        private string _username;
        byte[] _password;
        private int _score;
        private int _bestscore;
        private bool _admin;
        private int _id;

        public User() { }
        public User(long bitcoins, string username, byte[] password, int score, int bestscore, bool admin, int id) {
            _bitcoins = bitcoins;
            _username = username;
            _password = password;
            _score = score;
            _bestscore = bestscore;
            _admin = admin;
            _id = id;
        }

        [Required(ErrorMessage = "Ingrese numero valido de bitcoins")]
        [Range(0, double.PositiveInfinity, ErrorMessage = "Tiene que haber un numero positivo de bitcoins")]
        public long Bitcoins { get => _bitcoins; set => _bitcoins = value; }
        [Required(ErrorMessage = "Ingrese nombre valido")]
        public string Username { get => _username; set => _username = value; }
        public int Score { get => _score; set => _score = value; }
        [Required(ErrorMessage = "Ingrese puntaje valido")]
        [Range(0, double.PositiveInfinity, ErrorMessage = "El puntaje debe ser positivo")]
        public int Bestscore { get => _bestscore; set => _bestscore = value; }
        [Required]
        public bool Admin => _admin;
        public int id { get => _id; set => _id = value; }
        public byte[] Password { get => _password; set => _password = value; }

        public bool UpdateBitcoins(long bitcoins) {
            if (Bitcoins - bitcoins < 0 && Bitcoins < 0) {
                Bitcoins = 0;
            }

            Bitcoins = Bitcoins + bitcoins;

            DaB dab = new DaB();
            SqlConnection connection = dab.Connect();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_UpdateBitcoins";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", Username);
            command.Parameters.AddWithValue("@bitcoins", bitcoins);

            try {
                SqlDataReader reader = command.ExecuteReader();
                return Convert.ToBoolean(reader["code"]);
            }
            catch (Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message + "\nWrong username?");
                return false;
            }
        }

        public static User LoginUser(string username, string password) {
            byte[] hash = Utils.CreateMD5(password);

            DaB dab = new DaB();
            SqlConnection connection = dab.Connect();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_Login";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", hash);

            try {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) {
                    int code = Convert.ToInt32(reader["code"]);
                    if (code == 1) {
                        return new User(
                            Convert.ToInt64(reader["bitcoins"]),
                            reader["username"].ToString(),
                            (byte[])reader["password"],
                            0,
                            Convert.ToInt32(reader["bestscore"]),
                            Convert.ToBoolean(reader["admin"]),
                            Convert.ToInt32(reader["id"])
                        );
                    }
                }
                return null;
            }
            catch (Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message + "\nWrong credentials?");
                return null;
            }
        }

        public static User RegisterUser(string username, string password, bool admin) {
            byte[] hash = Utils.CreateMD5(password);

            DaB dab = new DaB();
            SqlConnection connection = dab.Connect();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_Register";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", hash);
            command.Parameters.AddWithValue("@admin", Convert.ToInt32(admin));

            try {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) {
                    int code = Convert.ToInt32(reader["code"]);
                    if (code == 1) {
                        return new User(1000000, username, hash, 0, 0, admin, -1);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                Console.WriteLine("Caught exception: " + ex.Message);
                return null;
            }
        }

        public static List<User> ListUsers() {
            List<User> users_list = new List<User>();
            Models.DaB daB = new Models.DaB();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_GetUsers";

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                User one_user = new User(
                    Convert.ToInt64(reader["bitcoins"]),
                    reader["username"].ToString(),
                    (byte[])reader["password"],
                    0,
                    Convert.ToInt32(reader["bestscore"]),
                    Convert.ToBoolean(reader["admin"]),
                    Convert.ToInt32(reader["id"])
                );
                users_list.Add(one_user);
            }

            return users_list;

        }

        public static User GetUser(int id) {
            Models.DaB daB = new Models.DaB();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_GetUser";
            command.CommandType = System.Data.CommandType.StoredProcedure;//Never forget pls
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                return new User(
                    Convert.ToInt64(reader["bitcoins"]),
                    reader["username"].ToString(),
                    (byte[])reader["password"],
                    0,
                    Convert.ToInt32(reader["bestscore"]),
                    Convert.ToBoolean(reader["admin"]),
                    Convert.ToInt32(reader["id"])
                );
            }
            return null;
        }

        public static void SaveUser(User u) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_EditUser";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", u.id);
            command.Parameters.AddWithValue("@name", u.Username);
            command.Parameters.AddWithValue("@password", u.Password);
            command.Parameters.AddWithValue("@bestscore", u.Bestscore);
            command.Parameters.AddWithValue("@bitcoins", u.Bitcoins);
            command.Parameters.AddWithValue("@admin", u.Admin);
            command.ExecuteNonQuery();
        }

        public static void DeleteUser(int id) {
            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_DeleteUser";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}