using System.Security.Cryptography;
using System.Text;

namespace OffersHub.Domain.Security
{
    public static class PasswordHasher
    {
        private const string SECRET_KEY = "TBC_IT_Academy_2025"; // Consider moving this to configuration

        public static string HashPassword(string password)
        {
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(password + SECRET_KEY);
                byte[] hashBytes = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
