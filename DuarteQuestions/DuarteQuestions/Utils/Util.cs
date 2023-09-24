using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.RegularExpressions;

namespace DuarteQuestions.Utils
{
    public static class Util
    {
        public const string EmailRegexPattern = "[a-z0-9._]+@[a-z]+\\.[a-z]{2,3}";

        public static bool IsEmail(string email)
        {
            Regex rx = new Regex(EmailRegexPattern);
            return rx.Match(email).Success;
        }

        public static string ToSHA256(string s)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                StringBuilder hash = new StringBuilder();
                byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
                foreach (byte b in hashArray)
                {
                    hash.Append(b.ToString("x"));
                }
                return hash.ToString();
            }
        }
    }
}
