using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreatePasswordHash("1234", out string passHash, out string pasSalt);

            Console.WriteLine("hash: " + passHash);
            Console.WriteLine("salt: " + pasSalt);

            bool confirm = VerifyPasswordHash("1234", passHash, pasSalt);

            Console.WriteLine(confirm);
        }

        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Convert.FromBase64String(password)));
            }
        }

        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            Console.WriteLine(password);
            using (var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt)))
            {
                string computedHash = Convert.ToBase64String(hmac.ComputeHash(Convert.FromBase64String(password)));

                if (storedHash.Equals(computedHash)) return true;
            }
            return false;
        }
    }
}
