using System;
using System.Text;

namespace FunitureApp.untils
{
	public class StringHelper
	{
		public static string BaseUrl = "http://192.168.1.11:5123";

        public StringHelper()
		{

		}
        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}

