using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Utilities
{
    public class TokenUtility
    {
        public static string getCustomerIdFromToken(string token, string privateKey)
        {
            byte[] secretKey = Encoding.UTF8.GetBytes(privateKey);
            string key = Jose.JWT.Decode(token, secretKey);
            string[] parts = key.Split(new char[] { ':' });
            return parts[0];
        }
    }
}
