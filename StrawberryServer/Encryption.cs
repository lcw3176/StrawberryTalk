using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryServer
{
    class Encryption
    {
        private byte[] hash;

        public void SetValue(string userPw)
        {
            using(HMACSHA256 sha = new HMACSHA256())
            {
                string key = Environment.GetEnvironmentVariable("ShaKey", EnvironmentVariableTarget.User);
                sha.Key = Encoding.UTF8.GetBytes(key);
                hash = sha.ComputeHash(Encoding.UTF8.GetBytes(userPw));                
            }
        }

        public string GetValue()
        {
            return Convert.ToBase64String(hash);
        }
    }
}
