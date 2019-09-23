using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Privacy.PGP
{
    /// <summary>
    /// This class to help user encrypt their data by PGP algorithm using Bouncy Castle
    /// </summary>
    public class PGPEncryptHelper
    {
        private static readonly string TempData = IoHelper.BasePath + @"\" + "tempData.doitsu";
        private static readonly string TempEncryptedPath = IoHelper.BasePath + @"\" + "pgp-encrypted.asc";

        public string PublicKeyData { get; set; }
        public string Data { get; set; }
        private string EncryptedData { get; set; }

        /// <summary>
        /// This constructor to drive user create new class
        /// </summary>
        /// <param name="publicKeyData">User's Public Key Data</param>
        /// <param name="data">User's Data</param>
        public PGPEncryptHelper(string publicKeyData, string data)
        {
            this.PublicKeyData = publicKeyData;
            this.Data = data;
        }

        public string GetEncryptedData()
        {
            if (this.EncryptedData == null)
            {
                var result = EncryptPgpStringData(this.Data);
                this.EncryptedData = result;
            }
            return EncryptedData;
        }


        private string EncryptPgpStringData(string inputData)
        {
            File.WriteAllText(TempData, inputData);
            // use armor: yes, use integrity check? yes?
            return EncryptPgpStringData(TempData, this.PublicKeyData, true, true);
        }

        private string EncryptPgpStringData(string inputFile, string publicKeyData, bool armor, bool withIntegrityCheck)
        {
            using (Stream publicKeyStream = IoHelper.GetStream(publicKeyData))
            {
                PgpPublicKey pubKey = ReadPublicKey(publicKeyStream);

                using (MemoryStream outputBytes = new MemoryStream())
                {
                    PgpCompressedDataGenerator dataCompressor = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                    PgpUtilities.WriteFileToLiteralData(dataCompressor.Open(outputBytes), PgpLiteralData.Binary, new FileInfo(inputFile));

                    dataCompressor.Close();
                    PgpEncryptedDataGenerator dataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom());

                    dataGenerator.AddMethod(pubKey);
                    byte[] dataBytes = outputBytes.ToArray();

                    using (Stream outputStream = File.Create(TempEncryptedPath))
                    {
                        if (armor)
                        {
                            using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
                            {
                                IoHelper.WriteStream(dataGenerator.Open(armoredStream, dataBytes.Length), ref dataBytes);
                            }
                        }
                        else
                        {
                            IoHelper.WriteStream(dataGenerator.Open(outputStream, dataBytes.Length), ref dataBytes);
                        }
                    }
                    return File.ReadAllText(TempEncryptedPath);
                }
            }
        }

        #region old encrypt
        //public void EncryptPgpFile(string inputFile, string outputFile)
        //{
        //    // use armor: yes, use integrity check? yes?
        //    EncryptPgpFile(inputFile, outputFile, PublicKeyPath, true, true);
        //}

        //public void EncryptPgpFile(string inputFile, string outputFile, string publicKeyFile, bool armor, bool withIntegrityCheck)
        //{
        //    using (Stream publicKeyStream = File.OpenRead(publicKeyFile))
        //    {
        //        PgpPublicKey pubKey = ReadPublicKey(publicKeyStream);

        //        using (MemoryStream outputBytes = new MemoryStream())
        //        {
        //            PgpCompressedDataGenerator dataCompressor = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
        //            PgpUtilities.WriteFileToLiteralData(dataCompressor.Open(outputBytes), PgpLiteralData.Binary, new FileInfo(inputFile));

        //            dataCompressor.Close();
        //            PgpEncryptedDataGenerator dataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom());

        //            dataGenerator.AddMethod(pubKey);
        //            byte[] dataBytes = outputBytes.ToArray();

        //            using (Stream outputStream = File.Create(outputFile))
        //            {
        //                if (armor)
        //                {
        //                    using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
        //                    {
        //                        IoHelper.WriteStream(dataGenerator.Open(armoredStream, dataBytes.Length), ref dataBytes);
        //                    }
        //                }
        //                else
        //                {
        //                    IoHelper.WriteStream(dataGenerator.Open(outputStream, dataBytes.Length), ref dataBytes);
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        private static PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);
            PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(inputStream);

            foreach (PgpPublicKeyRing keyRing in pgpPub.GetKeyRings())
            {
                foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                {
                    if (key.IsEncryptionKey)
                    {
                        return key;
                    }
                }
            }

            throw new ArgumentException("Can't find encryption key in key ring.");
        }


    }// end declare class
}// end namepsace
