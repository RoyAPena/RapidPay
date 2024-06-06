using RapidPay.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace RapidPay.Infrastructure.Services
{
    public class SecurityServices : ISecurityServices
    {
        //This method is used to tokenized the input string, is used for card number
        public string Tokenize(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a string.
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