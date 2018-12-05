using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web;

/*
 In case MVC is not installed run this command on Tools/Herramientas -> NuGeT -> Console:
    update-package -reinstall Microsoft.AspNet.Mvc
    Sauce:
    https://stackoverflow.com/questions/22325964/the-type-or-namespace-name-mvc-does-not-exist
     
     */

namespace quien_es_quien.Models {
    public class Utils {
        public static byte[] CreateMD5(string input) {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return hashBytes;
            }
        }

        public static SqlConnection Connect() {
            SqlConnection sql = new SqlConnection(@"Server=DESKTOP-FBKD222\SQLEXPRESS;Database=QEQC01;Trusted_Connection=True;");
            //SqlConnection sql = new SqlConnection("Server=10.128.8.16;Database=QEQC01;Uid=QEQC01;Pwd=QEQC01");
            sql.Open();
            return sql;
        }
    }
}