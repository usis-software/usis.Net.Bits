using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using usis.Platform;

namespace Playground
{
    internal static class Crypto
    {
        internal static void Main()
        {
            string s = "Dies ist ein Text. Und dies ist ein noch etwas längerer Text.";
            //string password2 = "1234567890123456";
            //string password1 = "12345678";
            //string s = "Testtext";

            //byte[] data = Crypto.EncryptData(ASCIIEncoding.ASCII.GetBytes(s), "paloalto");
            //Debug.Print("{0}", BitConverter.ToString(data));
            //data = Crypto.DecryptData(data, "paloalto");
            //Debug.Print("{0}", BitConverter.ToString(data));
            //s = ASCIIEncoding.ASCII.GetString(data);
            //Debug.Print(s);

            //s = s.Encrypt(password1);
            Debug.Print(s);
            //s = s.Decrypt(password2);
            Debug.Print(s);
        }

        static byte[] EncryptData(byte[] data, string password)
        {
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.Key = ASCIIEncoding.ASCII.GetBytes(password);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(password);

                using (var output = new MemoryStream())
                {
                    var encrypt = DES.CreateEncryptor();
                    var cryptoStream = new CryptoStream(output, encrypt, CryptoStreamMode.Write);
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();

                    return output.ToArray();
                }
            }
        }

        static byte[] DecryptData(byte[] data, string password)
        {
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.Key = ASCIIEncoding.ASCII.GetBytes(password);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(password);

                using (var input = new MemoryStream(data))
                {
                    var decrypt = DES.CreateDecryptor();
                    var cryptoStream = new CryptoStream(input, decrypt, CryptoStreamMode.Read);
                    var reader = new StreamReader(cryptoStream);
                    using (var output = new MemoryStream())
                    {
                        while (reader.Peek() >= 0)
                        {
                            output.WriteByte(Convert.ToByte(reader.Read()));
                        }
                        return output.ToArray();
                    }
                }
            }
        }

        #region Encrypt/Decrypt

        public static string Encrypt(string s, string password)
        {
            var bytes = ASCIIEncoding.ASCII.GetBytes(password);

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(
                        memoryStream,
                        cryptoProvider.CreateEncryptor(bytes, bytes),
                        CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cryptoStream);
                    writer.Write(s);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    writer.Flush();
                    return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
        }

        public static string Decrypt(string base64, string password)
        {
            var bytes = ASCIIEncoding.ASCII.GetBytes(password);

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64)))
                {
                    CryptoStream cryptoStream = new CryptoStream(
                        memoryStream,
                        cryptoProvider.CreateDecryptor(bytes, bytes),
                        CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cryptoStream);
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion Encrypt/Decrypt
    }
}
