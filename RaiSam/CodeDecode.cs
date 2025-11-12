using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RaiSam
{
    public class CodeDecode
    {
        public static string stringDecode(string a)
        {
            int l = a.Length, p;
            char[] k = new char[l];
            string s = "";
            for (int i = 0; i < l; i++)
            {
                p = (Convert.ToInt32(a[i]) - 1071) / 3;
                k[i] = Convert.ToChar(p);
                s = s + k[i].ToString();

            }
            return s;
        }

        public static string stringcode(string a)
        {
            int l = a.Length, p;
            char[] k = new char[l];
            string s = "";
            for (int i = 0; i < l; i++)
            {
                p = 3 * Convert.ToInt32(a[i]) + 1071;
                k[i] = Convert.ToChar(p);
                s = s + k[i].ToString();
            }

            return s;
        }

        //public static string GenerateHash(string value)
        //{
        //    SHA1 sha1 = SHA1.Create();
        //    //convert the input text to array of bytes
        //    if (value == null)
        //    {
        //        return "رمز عبور ضروری است.";
        //    }
        //    byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(value));

        //    //create new instance of StringBuilder to save hashed data
        //    StringBuilder returnValue = new StringBuilder();

        //    //loop for each byte and add it to StringBuilder
        //    for (int i = 0; i < hashData.Length; i++)
        //    {
        //        returnValue.Append(hashData[i].ToString("X2"));
        //    }

        //    // return hexadecimal string
        //    return returnValue.ToString();
        //}
        public static string ComputeSha256Hash(string value)
        {
            // Create a SHA256   
            if (value == null)
            {
                return "رمز عبور ضروری است.";
            }
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }  
    }
}