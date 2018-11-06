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
        public SqlConnection sql;
        public static string connectionString = @"Server=10.128.8.16;Database=QEQC01;Uid=QEQC01;Pwd=QEQC01";
        public static bool use_connection = true;
        public DaB() {
            try {
                Connect();
            }
            catch(Exception e) {
                System.Diagnostics.Debug.Print("Failed connection: " +e.Message+ " ("+e.Source+")");
                
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

 
    }
}