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
        //DESKTOP-FBKD222\SQLEXPRESS
        public SqlConnection sql;
        //public static string connectionString = @"Server=10.128.8.16;Database=QEQC01;Uid=QEQC01;Pwd=QEQC01";
        public static string connectionString = @"Server=DESKTOP-FBKD222\SQLEXPRESS;Database=QEQC01;Trusted_Connection=True;";
        public static bool use_connection = true;
        public DaB() {
            try {
                Connect();
            }
            catch (Exception e) {
                System.Diagnostics.Debug.Print("Failed connection: " + e.Message + " (" + e.Source + ")");

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
                try { sql.Close(); }
                catch { }
        }


    }
}