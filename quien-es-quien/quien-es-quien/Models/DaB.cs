using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

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
    }
}