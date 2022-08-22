using System;
using System.Security.Cryptography;
using System.Text;

namespace Crypto
{
    class Program
    {
        static string keySed = "hash_code";

        static void Main(string[] args)
        {
            Console.Write("(1) encode; (2) decode.\n Select option: ");
            int opt = Convert.ToInt32(Console.ReadLine());
            if (opt == 1)
            {
                Console.Write("Enter value for encode: ");
                string encode = (string)Console.ReadLine();
                Console.WriteLine("Hash encode: " + Encrypt(encode));
            }
            else if (opt == 2)
            {
                Console.Write("Enter value for decode: ");
                string decode = (string)Console.ReadLine();
                Console.WriteLine("Hash decode: " + Decrypt(decode));
            }
            else
            {
                Console.WriteLine("Error execution.");
            }
        }

        public static string Encrypt(string text)
        {
            using (var hashMD5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = hashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(keySed));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textByt = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] byt = transform.TransformFinalBlock(textByt, 0, textByt.Length);
                        return Convert.ToBase64String(byt, 0, byt.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(keySed));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] encBytes = Convert.FromBase64String(cipher);
                        byte[] byt = transform.TransformFinalBlock(encBytes, 0, encBytes.Length);
                        return UTF8Encoding.UTF8.GetString(byt);
                    }
                }
            }
        }
    }
}
