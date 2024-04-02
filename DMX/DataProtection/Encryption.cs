using System.Text;

namespace DMX.DataProtection
{
    public static class Encryption
    {

        public static string Encrypt(string ToEncrypt)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(ToEncrypt));
        }
        public static string Decrypt(string cypherString)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(cypherString));
        }
    }
}

