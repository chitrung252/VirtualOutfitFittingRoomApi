using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Privacy.RSA
{
    public class RSAHelper
    {
        public static string EncryptPKCS8(string data, string publicKey)
        {



            Asn1Object obj = Asn1Object.FromByteArray(Convert.FromBase64String(publicKey));
            DerSequence publicKeySequence = (DerSequence)obj;
            DerBitString encodedPublicKey = (DerBitString)publicKeySequence[1];

            DerSequence publicKeyDer = (DerSequence)Asn1Object.FromByteArray(encodedPublicKey.GetBytes());
            DerInteger modulus = (DerInteger)publicKeyDer[0];
            DerInteger exponent = (DerInteger)publicKeyDer[1];

            RsaKeyParameters keyParameters = new RsaKeyParameters(false, modulus.PositiveValue, exponent.PositiveValue);
            //Then, BouncyCastle provides an easy way to convert this to.NET compatible RSAParameters:
            RSAParameters parameters = DotNetUtilities.ToRSAParameters(keyParameters);
            //You can then easily import the key parameters into RSACryptoServiceProvider:
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);
            //Finally, do your encryption:
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(data);
            // Sign data with Pkcs1
            byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);
            // Convert Bytes to Hash
            var hash = Convert.ToBase64String(encryptedData);

            return hash;
        }

        public class RSAKeyPair
        {
            public string RSAPublicKey { get; set; }
            public string RSAPrivateKey { get; set; }
        }
        public class RSASkyConnectHelper
        {
            private static int keySizeInBits = 2048;
            public static string Encrypt(string data, string publicKey)
            {
                var encryptedText = "";
                //converting it back
                {
                    //get a stream from the string
                    var sr = new System.IO.StringReader(publicKey);
                    //we need a deserializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //get the object back from the stream
                    var pubKey = (RSAParameters)xs.Deserialize(sr);

                    using (var csp = new RSACryptoServiceProvider())
                    {
                        //conversion for the private key is no black magic either ... omitted
                        //we have a public key ... let's get a new csp and load that key
                        csp.ImportParameters(pubKey);

                        //we need some data to encrypt
                        var plainTextData = data;

                        //for encryption, always handle bytes...
                        var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

                        //apply pkcs#1.5 padding and encrypt our data 
                        var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);
                        //we might want a string representation of our cypher text... base64 will do
                        encryptedText = Convert.ToBase64String(bytesCypherText);
                    }
                }
                return encryptedText;
            }

            public static string EncryptFromXML(string data, string publicKey)
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                string result = null;
                using (var rsa = new RSACryptoServiceProvider(2048)) // or 4096, base on key length
                {
                    try
                    {
                        // Client encrypting data with public key issued by server
                        // "publicKey" must be XML format, use https://superdry.apphb.com/tools/online-rsa-key-converter
                        // to convert from PEM to XML before hash
                        rsa.FromXmlString(publicKey);
                        var encryptedData = rsa.Encrypt(dataBytes, false);
                        var base64Encrypted = Convert.ToBase64String(encryptedData);
                        result = base64Encrypted;
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }

                }

                return result;
            }

            public static RSAKeyPair GenerateKeys()
            {
                //lets take a new CSP with a new 2048 bit rsa key pair
                var csp = new RSACryptoServiceProvider(2048);

                //how to get the private key
                var privKey = csp.ExportParameters(true);

                //and the public key ...
                var pubKey = csp.ExportParameters(false);

                //converting the public key into a string representation
                string pubKeyString;
                {
                    //we need some buffer
                    var sw = new System.IO.StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, pubKey);
                    //get the string from the stream
                    pubKeyString = sw.ToString();
                }

                string privKeyString;
                {
                    //we need some buffer
                    var sw = new System.IO.StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, privKey);
                    //get the string from the stream
                    privKeyString = sw.ToString();
                }

                return new RSAKeyPair()
                {
                    RSAPrivateKey = privKeyString,
                    RSAPublicKey = pubKeyString

                };

            }



            public static string Decrypt(string data, string privateKey)
            {
                //converting it back
                {
                    //get a stream from the string
                    var sr = new System.IO.StringReader(privateKey);
                    //we need a deserializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //get the object back from the stream
                    var privKey = (RSAParameters)xs.Deserialize(sr);

                    using (var csp = new RSACryptoServiceProvider())
                    {
                        csp.ImportParameters(privKey);
                        //decrypt and strip pkcs#1.5 padding
                        ////first, get our bytes back from the base64 string ...
                        var bytesPlainTextData = csp.Decrypt(Convert.FromBase64String(data), false);

                        //get our original plainText back...
                        return System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
                    }
                }
            }

        }

    }
}
