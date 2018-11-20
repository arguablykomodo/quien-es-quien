using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace quien_es_quien.Models
{
    public class User
    {
        private long _bitcoins;
        private readonly string _username;
        private int _score;
        private int _bestscore;
        private readonly bool _admin;
        private int _id;

        public User(long bitcoins, string username, int score, int bestscore, bool admin)
        {
            _bitcoins = bitcoins;
            _username = username;
            _score = score;
            _bestscore = bestscore;
            _admin = admin;
            _id = -1;
        }

        public User(long bitcoins, string username, int score, int bestscore, bool admin, int id)
        {
            _bitcoins = bitcoins;
            _username = username;
            _score = score;
            _bestscore = bestscore;
            _admin = admin;
            _id = id;
        }

        public long Bitcoins { get => _bitcoins; set => _bitcoins = value; }
        public string Username => _username;
        public int Score { get => _score; set => _score = value; }
        public int Bestscore { get => _bestscore; set => _bestscore = value; }
        public bool Admin => _admin;
        public int id { get => _id; set => _id = value; }

        public bool UpdateBitcoins(long bitcoins)
        {
            if (Bitcoins - bitcoins < 0 && Bitcoins < 0)
            {
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

            try
            {
                SqlDataReader reader = command.ExecuteReader();
                return Convert.ToBoolean(reader["code"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message + "\nWrong username?");
                return false;
            }
        }

        public static User LoginUser(string username, string password)
        {
            byte[] hash = Utils.CreateMD5(password);

            DaB dab = new DaB();
            SqlConnection connection = dab.Connect();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_Login";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", hash);

            try
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int code = Convert.ToInt32(reader["code"]);
                    if (code == 1)
                    {
                        string uname = reader["username"].ToString();
                        long bitcoins = Convert.ToInt64(reader["bitcoins"]);
                        int bestscore = Convert.ToInt32(reader["bestscore"]);
                        bool admin = Convert.ToBoolean(reader["admin"]);
                        return new User(bitcoins, uname, 0, bestscore, admin);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message + "\nWrong credentials?");
                return null;
            }
        }

        public static User RegisterUser(string username, string password, bool admin)
        {
            byte[] hash = Utils.CreateMD5(password);

            DaB dab = new DaB();
            SqlConnection connection = dab.Connect();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_Register";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", hash);
            command.Parameters.AddWithValue("@admin", Convert.ToInt32(admin));

            try
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int code = Convert.ToInt32(reader["code"]);
                    if (code == 1)
                    {
                        return new User(1000000, username, 0, 0, admin);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message);
                return null;
            }
        }

        public static List<User> ListUsers()
        {
            List<User> users_list = new List<User>();
            Models.DaB daB = new Models.DaB();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_GetUsers";

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                User one_user = new User((long)reader["bitcoins"], reader["username"].ToString(), 0, (int)reader["bestscore"], reader["admin"].ToString() == "1", (int)reader["id"]);
                users_list.Add(one_user);
            }

            return users_list;

        }
        
        public static User GetUser(int id)
        {
            Models.DaB daB = new Models.DaB();

            SqlConnection c = new DaB().Connect();
            SqlCommand command = c.CreateCommand();
            command.CommandText = "sp_GetUser";
            command.CommandType = System.Data.CommandType.StoredProcedure;//Never forget pls
            command.Parameters.AddWithValue("id", id);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                return new User((long)reader["bitcoins"], reader["username"].ToString(), 0, (int)reader["bestscore"], reader["admin"].ToString() == "1", (int)reader["id"]);
            }
            return null;
        }

        public static bool SaveUser(User u)
        {
            return false;
        }
    }
}