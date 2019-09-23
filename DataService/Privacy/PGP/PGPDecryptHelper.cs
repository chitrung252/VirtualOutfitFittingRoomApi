using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Privacy.PGP
{
    public class PGPDecryptHelper
    {
        public string PrivateKeyData { get; set; }
        public string EncryptedData { get; set; }
        public string Password { get; set; }
        private string DecryptedData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="privateKeyData">Private Key string</param>
        /// <param name="encryptedData">Encrypted data</param>
        public PGPDecryptHelper(string privateKeyData,string password, string encryptedData)
        {
            this.PrivateKeyData = privateKeyData;
            this.EncryptedData = encryptedData;
            this.Password = password;
        }

        public string GetDecryptedData()
        {
            if (this.DecryptedData == null)
            {
                var result = DecryptPgpData(this.EncryptedData);
                this.DecryptedData = result;
            }
            return this.DecryptedData;
        }

        private string DecryptPgpData(string inputData)
        {
            string output;
            using (Stream inputStream = IoHelper.GetStream(inputData))
            {
                using (Stream keyIn = IoHelper.GetStream(PrivateKeyData))
                {
                    output = DecryptPgpData(inputStream, keyIn, Password);
                }
            }
            return output;
        }

        private string DecryptPgpData(Stream inputStream, Stream privateKeyStream, string passPhrase)
        {
            string output;

            PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(inputStream));
            // find secret key
            PgpSecretKeyRingBundle pgpKeyRing = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));

            PgpObject pgp = null;
            if (pgpFactory != null)
            {
                pgp = pgpFactory.NextPgpObject();
            }

            // the first object might be a PGP marker packet.
            PgpEncryptedDataList encryptedData = null;
            if (pgp is PgpEncryptedDataList)
            {
                encryptedData = (PgpEncryptedDataList)pgp;
            }
            else
            {
                encryptedData = (PgpEncryptedDataList)pgpFactory.NextPgpObject();
            }

            // decrypt
            PgpPrivateKey privateKey = null;
            PgpPublicKeyEncryptedData pubKeyData = null;
            foreach (PgpPublicKeyEncryptedData pubKeyDataItem in encryptedData.GetEncryptedDataObjects())
            {
                privateKey = FindSecretKey(pgpKeyRing, pubKeyDataItem.KeyId, passPhrase.ToCharArray());

                if (privateKey != null)
                {
                    pubKeyData = pubKeyDataItem;
                    break;
                }
            }

            if (privateKey == null)
            {
                throw new ArgumentException("Secret key for message not found.");
            }

            PgpObjectFactory plainFact = null;
            using (Stream clear = pubKeyData.GetDataStream(privateKey))
            {
                plainFact = new PgpObjectFactory(clear);
            }

            PgpObject message = plainFact.NextPgpObject();

            if (message is PgpCompressedData)
            {
                PgpCompressedData compressedData = (PgpCompressedData)message;
                PgpObjectFactory pgpCompressedFactory = null;

                using (Stream compDataIn = compressedData.GetDataStream())
                {
                    pgpCompressedFactory = new PgpObjectFactory(compDataIn);
                }

                message = pgpCompressedFactory.NextPgpObject();
                PgpLiteralData literalData = null;
                if (message is PgpOnePassSignatureList)
                {
                    message = pgpCompressedFactory.NextPgpObject();
                }

                literalData = (PgpLiteralData)message;
                using (Stream unc = literalData.GetInputStream())
                {
                    output = IoHelper.GetString(unc);
                }

            }
            else if (message is PgpLiteralData)
            {
                PgpLiteralData literalData = (PgpLiteralData)message;
                using (Stream unc = literalData.GetInputStream())
                {
                    output = IoHelper.GetString(unc);
                }
            }
            else if (message is PgpOnePassSignatureList)
            {
                throw new PgpException("Encrypted message contains a signed message - not literal data.");
            }
            else
            {
                throw new PgpException("Message is not a simple encrypted file - type unknown.");
            }

            return output;
        }
        private static PgpPrivateKey FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass)
        {
            PgpSecretKey pgpSecKey = pgpSec.GetSecretKey(keyId);
            if (pgpSecKey == null)
            {
                return null;
            }

            return pgpSecKey.ExtractPrivateKey(pass);
        }
    }
}
