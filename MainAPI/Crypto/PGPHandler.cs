using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace SkyConnect.API.Crypto
{
    public class PGPHandler
    {
        private static readonly int BUFFER_SIZE = 1 << 16;
        private static readonly String DEFAULT_SERVICE_CODE = "ONE_SERVICE";
        private static readonly Dictionary<String, PGPHandler> map = new Dictionary<string, PGPHandler>();

        private PgpPublicKey publicKey;
        private PgpSecretKeyRingBundle pgpSec;
        private PgpSecretKey secretKey;
        private char[] password;

        private PGPHandler(String publicKeyPath, String privateKeyPath, String password)
        {
            using (Stream puStream = new FileStream(publicKeyPath, FileMode.Open))
            using (Stream prStream = new FileStream(privateKeyPath, FileMode.Open))
            {
                try
                {
                    this.publicKey = readPublicKey(puStream);
                    this.pgpSec = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(prStream));
                    this.secretKey = readSecretKey(pgpSec);
                    this.password = password.ToCharArray();
                }
                catch (IOException ex)
                {
                    throw new CryptoException(ex.Message, ex);
                }
                catch (PgpException ex)
                {
                    throw new CryptoException(ex.Message, ex);
                }
            }

        }

        public static void Init(String publicKeyPath, String privateKeyString, String password)
        {
            Init(DEFAULT_SERVICE_CODE, publicKeyPath, privateKeyString, password);
        }

        private static void Init(String serviceCode, String publicKeyPath, String privateKeyString, String password)
        {
            try
            {
                map.Add(serviceCode, new PGPHandler(publicKeyPath, privateKeyString, password));
            }
            catch (Exception ex)
            {
                map[serviceCode] = new PGPHandler(publicKeyPath, privateKeyString, password);
            }
        }

        public static PGPHandler GetInstance()
        {
            return GetInstance(DEFAULT_SERVICE_CODE);
        }

        public static PGPHandler GetInstance(string serviceCode)
        {
            return map[serviceCode];
        }

        private PgpSignatureGenerator createSignatureGenerator()
        {
            PgpPrivateKey privateKey = secretKey.ExtractPrivateKey(password);
            PgpPublicKey internalPublicKey = secretKey.PublicKey;
            PgpSignatureGenerator signatureGenerator = new PgpSignatureGenerator(internalPublicKey.Algorithm, HashAlgorithmTag.Sha1);
            signatureGenerator.InitSign(PgpSignature.BinaryDocument, privateKey);
            var userIds = internalPublicKey.GetUserIds();
            foreach (var userId in userIds)
            {
                PgpSignatureSubpacketGenerator spGen = new PgpSignatureSubpacketGenerator();
                spGen.SetSignerUserId(false, (String)userId);
                signatureGenerator.SetHashedSubpackets(spGen.Generate());
                break;
            }
            return signatureGenerator;
        }

        public static void WriteToLiteralData(Stream outStream, char filetype, byte[] data)
        {
            PgpLiteralDataGenerator lData = new PgpLiteralDataGenerator();
            Stream pOut = lData.Open(outStream, filetype, "temp", data.LongLength, new DateTime());
            foreach (var b in data)
            {
                pOut.WriteByte(b);
            }
            lData.Close();
        }

        private static void WriteToLiteralData(PgpSignatureGenerator signatureGenerator, Stream outStream, byte[] data)
        {
            PgpLiteralDataGenerator lData = new PgpLiteralDataGenerator();
            MemoryStream ms = new MemoryStream(data);
            try
            {
                using (Stream literalOut = lData.Open(outStream, PgpLiteralData.Binary, "pgp", DateTime.UtcNow,
                    new byte[BUFFER_SIZE]))
                {
                    byte[] buf = new byte[BUFFER_SIZE];
                    int len;
                    while ((len = ms.Read(buf, 0, buf.Length)) > 0)
                    {
                        literalOut.Write(buf, 0, len);
                        signatureGenerator.Update(buf, 0, len);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptoException(ex.Message, ex);
            }
            finally
            {
                lData.Close();
            }
        }

        public void EncryptAndSign(byte[] data, Stream outStream)
        {
            try
            {
                outStream = new ArmoredOutputStream(outStream);
                PgpEncryptedDataGenerator encryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, new SecureRandom());
                encryptedDataGenerator.AddMethod(publicKey);
                PgpCompressedDataGenerator compressedData = null;
                try
                {
                    Stream encryptedOut = encryptedDataGenerator.Open(outStream, new byte[BUFFER_SIZE]);
                    compressedData = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                    try
                    {
                        Stream compressedOut = compressedData.Open(encryptedOut);
                        PgpSignatureGenerator signatureGenerator = createSignatureGenerator();
                        signatureGenerator.GenerateOnePassVersion(false).Encode(compressedOut);
                        WriteToLiteralData(signatureGenerator, compressedOut, data);
                        signatureGenerator.Generate().Encode(compressedOut);
                        compressedOut.Close();
                    }
                    catch (Exception e)
                    {
                    }
                    encryptedOut.Close();
                }
                finally
                {
                    if (compressedData != null)
                    {
                        compressedData.Close();
                    }
                    try
                    {
                        encryptedDataGenerator.Close();
                    }
                    catch (IOException e)
                    {
                    }
                    outStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw new CryptoException(ex.Message, ex);
            }
        }

        private PgpPrivateKey FindSecretKey(long keyId)
        {
            PgpSecretKey pgpSecKey = pgpSec.GetSecretKey(keyId);
            if (pgpSecKey == null)
            {
                return null;
            }
            return pgpSecKey.ExtractPrivateKey(password);
        }

        public String DecryptAndVerifySignature(byte[] encryptData, Stream decryptData)
        {
            try
            {
                var bais = PgpUtilities.GetDecoderStream(new MemoryStream(encryptData));
                PgpObjectFactory objectFactory = new PgpObjectFactory(bais);
                var firstObject = objectFactory.NextPgpObject();
                PgpEncryptedDataList dataList =
                    (PgpEncryptedDataList)(firstObject is PgpEncryptedDataList ? firstObject : objectFactory.NextPgpObject());
                PgpPrivateKey privateKey = null;
                PgpPublicKeyEncryptedData encryptedData = null;
                var list = dataList.GetEncryptedDataObjects();
                foreach (var obj in list)
                {
                    if (privateKey != null)
                    {
                        break;
                    }
                    encryptedData = (PgpPublicKeyEncryptedData)obj;
                    privateKey = FindSecretKey(encryptedData.KeyId);
                }
                if (privateKey == null || encryptedData == null)
                {
                    throw new ArgumentException("secret key for message not found.");
                }
                Stream clear = encryptedData.GetDataStream(privateKey);
                PgpObjectFactory clearObjectFactory = new PgpObjectFactory(clear);
                var message = clearObjectFactory.NextPgpObject();
                if (message is PgpCompressedData)
                {
                    PgpCompressedData cData = (PgpCompressedData)message;
                    objectFactory = new PgpObjectFactory(cData.GetDataStream());
                    message = objectFactory.NextPgpObject();
                }

                PgpOnePassSignature calculatedSignature = null;
                if (message is PgpOnePassSignatureList)
                {
                    calculatedSignature = ((PgpOnePassSignatureList)message)[0];
                    calculatedSignature.InitVerify(publicKey);
                    message = objectFactory.NextPgpObject();
                }

                var baos = new MemoryStream();
                if (message is PgpLiteralData)
                {
                    PgpLiteralData ld = (PgpLiteralData)message;
                    Stream unc = ld.GetInputStream();
                    int ch;
                    while ((ch = unc.ReadByte()) >= 0)
                    {
                        if (calculatedSignature != null)
                        {
                            calculatedSignature.Update((byte)ch);
                        }
                        baos.WriteByte((byte)ch);
                    }
                }
                else if (message is PgpOnePassSignatureList)
                {
                    throw new PgpException("encrypted message contains a signed message - not literal data.");
                }
                else
                {
                    throw new PgpException("message is not a simple encrypted file - type unknown.");
                }
                return Encoding.UTF8.GetString(baos.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private PgpPublicKey readPublicKey(Stream stream)
        {
            stream = PgpUtilities.GetDecoderStream(stream);
            PgpPublicKeyRingBundle pkBun = new PgpPublicKeyRingBundle(stream);
            var pkRings = pkBun.GetKeyRings();
            foreach (var ring in pkRings)
            {
                var pks = ((PgpPublicKeyRing)ring).GetPublicKeys();
                foreach (var key in pks)
                {
                    if (((PgpPublicKey)key).IsEncryptionKey)
                    {
                        return (PgpPublicKey)key;
                    }
                }
            }
            throw new ArgumentException("Invalid public key");
        }

        private PgpSecretKey readSecretKey(PgpSecretKeyRingBundle bundle)
        {
            var keyRings = bundle.GetKeyRings();
            foreach (var keyRing in keyRings)
            {
                if (keyRing is PgpSecretKeyRing)
                {
                    return ((PgpSecretKeyRing)keyRing).GetSecretKey();
                }
            }
            throw new ArgumentException("Secret Key for message not found");
        }
    }
}