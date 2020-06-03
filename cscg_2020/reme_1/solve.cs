using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace reme_1_solve
{
    class Program
    {
        static void Main(string[] args)
        {
            // ReMe Part I
            Console.WriteLine(StringEncryption.Decrypt("D/T9XRgUcKDjgXEldEzeEsVjIcqUTl7047pPaw7DZ9I="));
        }

        private class StringEncryption
        {
            public static string Encrypt(string clearText)
            {
                string password = "A_Wise_Man_Once_Told_Me_Obfuscation_Is_Useless_Anyway";
                byte[] bytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
                    {
                73,
                118,
                97,
                110,
                32,
                77,
                101,
                100,
                118,
                101,
                100,
                101,
                118
                    });
                    aes.Key = rfc2898DeriveBytes.GetBytes(32);
                    aes.IV = rfc2898DeriveBytes.GetBytes(16);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(bytes, 0, bytes.Length);
                            cryptoStream.Close();
                        }
                        clearText = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                return clearText;
            }

            public static string Decrypt(string cipherText)
            {
                string password = "A_Wise_Man_Once_Told_Me_Obfuscation_Is_Useless_Anyway";
                cipherText = cipherText.Replace(" ", "+");
                byte[] array = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
                    {
                73,
                118,
                97,
                110,
                32,
                77,
                101,
                100,
                118,
                101,
                100,
                101,
                118
                    });
                    aes.Key = rfc2898DeriveBytes.GetBytes(32);
                    aes.IV = rfc2898DeriveBytes.GetBytes(16);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(array, 0, array.Length);
                            cryptoStream.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
                    }
                }
                return cipherText;
            }
        }
    }
}
