using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
             var passwordResult = CreatePasswordHash("1234").Result;

            Console.WriteLine("hash: " + passwordResult.passwordHash);
            Console.WriteLine("salt: " + passwordResult.passwordSalt);

            bool confirm = VerifyPasswordHash("1234", passwordResult.passwordHash, passwordResult.passwordSalt).Result;

            Console.WriteLine(confirm);
        }
        
        private static async Task<(string passwordSalt, string passwordHash)> CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                return (Convert.ToBase64String(hmac.Key),
                        Convert.ToBase64String(await hmac.ComputeHashAsync(
                            new MemoryStream(Convert.FromBase64String(password)
                        ))));
            }
        }

        private static async Task<bool> VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt)))
            {
                string computedHash = Convert.ToBase64String(await hmac.ComputeHashAsync(new MemoryStream(Convert.FromBase64String(password))));

                Console.WriteLine(computedHash);

                if (storedHash.Equals(computedHash)) return true;
            }
            return false;
        }
    }
}
