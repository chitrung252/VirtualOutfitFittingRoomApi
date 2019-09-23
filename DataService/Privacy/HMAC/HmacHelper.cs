using DataService.Models;
using DataService.Utilities;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Privacy.HMAC
{
    public class HmacHelper
    {
        public static string CalculateMac(Models.HMACEnum algorithm = Models.HMACEnum.HMACSHA256, string key = "", string message = "", bool toHex = true)
        {
            byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            byte[] hashMessage = null;
            switch (algorithm)
            {
                case Models.HMACEnum.HMACMD5:
                    hashMessage = new HMACMD5(keyByte).ComputeHash(messageBytes);
                    break;
                case Models.HMACEnum.HMACSHA1:
                    hashMessage = new HMACSHA1(keyByte).ComputeHash(messageBytes);
                    break;
                case Models.HMACEnum.HMACSHA256:
                    hashMessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
                    break;
                case Models.HMACEnum.HMACSHA512:
                    hashMessage = new HMACSHA512(keyByte).ComputeHash(messageBytes);
                    break;
                default:
                    hashMessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
                    break;
            }
            string sOut = "";
            if (toHex)
            {
                sOut = BitConverter.ToString(hashMessage);
                sOut = sOut.Replace("-", "");
                sOut = sOut.ToLower();
            }
            else
            {
                // to lowercase hexits
                String.Concat(Array.ConvertAll(hashMessage, x => x.ToString("x2")));

                // to base64
                sOut = Convert.ToBase64String(hashMessage).ToLower();
            }
            return sOut;
        }
    }
}
