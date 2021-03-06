using System;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;

namespace reme_2
{
    class solve
    {
        static void Main(string[] args)
        {
            // Extraced by hand from Ghidra ByteView
            String encodedPath = "encoded_function";
            byte[] encoded = File.ReadAllBytes(encodedPath);

            // Extracted by hand from Ghidra ByteView
            byte[] key = { 0x00, 0x28, 0x10, 0x00, 0x00, 0x06, 0x00, 0x28, 0x2f, 0x00, 0x00, 0x0a, 0x0c, 0x08, 0x2c, 0x14, 0x00, 0x72, 0xc3, 0x00, 0x00, 0x70, 0x28, 0x30, 0x00, 0x00, 0x0a, 0x00, 0x15, 0x28, 0x31, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x17, 0x0a, 0x28, 0x32, 0x00, 0x00, 0x0a, 0x6f, 0x33, 0x00, 0x00, 0x0a, 0x12, 0x00, 0x28, 0x08, 0x00, 0x00, 0x06, 0x26, 0x06, 0x0d, 0x09, 0x2c, 0x14, 0x00, 0x72, 0xc3, 0x00, 0x00, 0x70, 0x28, 0x30, 0x00, 0x00, 0x0a, 0x00, 0x15, 0x28, 0x31, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x28, 0x09, 0x00, 0x00, 0x06, 0x13, 0x04, 0x11, 0x04, 0x2c, 0x14, 0x00, 0x72, 0xc3, 0x00, 0x00, 0x70, 0x28, 0x30, 0x00, 0x00, 0x0a, 0x00, 0x15, 0x28, 0x31, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x02, 0x8e, 0x16, 0xfe, 0x01, 0x13, 0x05, 0x11, 0x05, 0x2c, 0x14, 0x00, 0x72, 0xcd, 0x00, 0x00, 0x70, 0x28, 0x30, 0x00, 0x00, 0x0a, 0x00, 0x15, 0x28, 0x31, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x02, 0x16, 0x9a, 0x72, 0x11, 0x01, 0x00, 0x70, 0x28, 0x13, 0x00, 0x00, 0x06, 0x28, 0x34, 0x00, 0x00, 0x0a, 0x13, 0x06, 0x11, 0x06, 0x2c, 0x16, 0x00, 0x72, 0xc3, 0x00, 0x00, 0x70, 0x28, 0x30, 0x00, 0x00, 0x0a, 0x00, 0x15, 0x28, 0x31, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x2b, 0x10, 0x00, 0x72, 0x6b, 0x01, 0x00, 0x70, 0x02, 0x16, 0x9a, 0x28, 0x35, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x72, 0xe3, 0x01, 0x00, 0x70, 0x28, 0x0a, 0x00, 0x00, 0x06, 0x0b, 0x07, 0x7e, 0x36, 0x00, 0x00, 0x0a, 0x28, 0x37, 0x00, 0x00, 0x0a, 0x13, 0x07, 0x11, 0x07, 0x2c, 0x37, 0x00, 0x07, 0x72, 0xfd, 0x01, 0x00, 0x70, 0x28, 0x0b, 0x00, 0x00, 0x06, 0x13, 0x08, 0x11, 0x08, 0x28, 0x38, 0x00, 0x00, 0x0a, 0x20, 0xe9, 0x00, 0x00, 0x00, 0xfe, 0x01, 0x13, 0x09, 0x11, 0x09, 0x2c, 0x14, 0x00, 0x72, 0x33, 0x02, 0x00, 0x70, 0x28, 0x30, 0x00, 0x00, 0x0a, 0x00, 0x15, 0x28, 0x31, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x00, 0x2a };

            byte[] rawAssembly = AES_Decrypt(encoded, key);
            // Check that it is valid code
            Assembly.Load(rawAssembly).GetTypes()[0].GetMethod("Check", BindingFlags.Static | BindingFlags.Public);

            File.WriteAllBytes("tmp.exe", rawAssembly);
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] result = null;
            byte[] salt = new byte[8]
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8
            };
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.KeySize = 256;
                    rijndaelManaged.BlockSize = 128;
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Mode = CipherMode.CBC;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cryptoStream.Close();
                    }
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] result = null;
            byte[] salt = new byte[8]
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8
            };
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.KeySize = 256;
                    rijndaelManaged.BlockSize = 128;
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Mode = CipherMode.CBC;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cryptoStream.Close();
                    }
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }
    }
}
