using System.Security.Cryptography;
using System.Text;

namespace WebApplicationEmail.Utilities
{
    public static class Encrypter
    {
        public static string EncryptPassword(string password, string username)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", username, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }


    }
}
