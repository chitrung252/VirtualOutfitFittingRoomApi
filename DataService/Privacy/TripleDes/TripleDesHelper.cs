using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Privacy.TripleDes
{
    public class TripleDesHelper
    {
        public string DesKey { get; set; }
        public string DesVector { get; set; }
        public TripleDESCryptoServiceProvider TripleDES { get; set; }

        public TripleDesHelper()
        {
            TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.GenerateIV();
            TripleDES.GenerateKey();
            DesKey = Convert.ToBase64String(TripleDES.Key);
            DesVector = Convert.ToBase64String(TripleDES.IV);
        }

        public static string DecryptString3Des(string encryptedStr, string keyStr, string IVStr)
        {
            var encryptedText = Convert.FromBase64String(encryptedStr);
            var key = Convert.FromBase64String(keyStr);
            var IV = Convert.FromBase64String(IVStr);
            string text = null;

            using (TripleDESCryptoServiceProvider tdsAlg = new TripleDESCryptoServiceProvider())
            {
                tdsAlg.Key = key;
                tdsAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = tdsAlg.CreateDecryptor(tdsAlg.Key, tdsAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(encryptedText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            text = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return text;
        }

        /// <summary>
        /// Encrypt Data by tripleDes
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string EncryptString3Des(string plainText, string keyStr, string IVStr)
        {
            var key = Convert.FromBase64String(keyStr);
            var IV = Convert.FromBase64String(IVStr);

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("key");
            byte[] encrypted;
            // Create an TripleDESCryptoServiceProvider object
            // with the specified key and IV.
            using (TripleDESCryptoServiceProvider tdsAlg = new TripleDESCryptoServiceProvider())
            {
                tdsAlg.Key = key;
                tdsAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = tdsAlg.CreateEncryptor(tdsAlg.Key, tdsAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

    }
}
