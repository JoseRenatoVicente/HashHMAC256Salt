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
            var passwordResult = CreatePasswordHash("1234A!@#").Result;

            Console.WriteLine("hash: " + passwordResult.passwordHash);
            Console.WriteLine("salt: " + passwordResult.passwordSalt);

            var confirm = VerifyPasswordHash("1234A!@#", passwordResult.passwordHash, passwordResult.passwordSalt).Result;

            Console.WriteLine(confirm);
        }

        private static async Task<(string passwordSalt, string passwordHash)> CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA256();
            return (Convert.ToBase64String(hmac.Key),
                Convert.ToBase64String(await hmac.ComputeHashAsync(
                    new MemoryStream(Encoding.ASCII.GetBytes(password)
                    ))));
        }

        private static async Task<bool> VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
            var computedHash = Convert.ToBase64String(await hmac.ComputeHashAsync(new MemoryStream(Encoding.ASCII.GetBytes(password))));

            Console.WriteLine(computedHash);

            return storedHash.Equals(computedHash);
        }
    }
}