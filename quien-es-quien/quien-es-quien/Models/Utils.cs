using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/*
 In case MVC is not installed run this command on Tools/Herramientas -> NuGeT -> Console:
    update-package -reinstall Microsoft.AspNet.Mvc
    Sauce:
    https://stackoverflow.com/questions/22325964/the-type-or-namespace-name-mvc-does-not-exist
     
     */

namespace quien_es_quien.Models {
    public class Utils {
        public static string CreateMD5(string input) {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}