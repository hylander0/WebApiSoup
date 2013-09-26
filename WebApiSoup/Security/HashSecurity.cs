using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApiSoup.Security
{
    public class HashSecurity
    {
        static MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();


        //ONLY USE FOR TESTING
        public static string Encrypt(string key, string secret)
        {


            string retVal = string.Empty;
            string tm = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            //Console.WriteLine(String.Format("Encrypted with Sec : {0}", tm));
            string toEncrypt = string.Format("{0}{1}{2}", key, secret, tm);
            try
            {
                return md5hex(toEncrypt);
            }
            catch (Exception ex)
            {
                //throw new EncryptionException(EncryptionException.Code.EncryptionFailure, ex, MethodBase.GetCurrentMethod()); 
                throw ex;
            }
        }

        public static bool IsTokenValid(string validAppKey, string validSecret, string TokenToTest)
        {
            int tm = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            //Console.WriteLine(String.Format("Decrypted with Sec: {0}", tm));


            string hashToTest;
            for (int s = 0; s <= 300; s++) //Loop through all posibilities, start from current time and expand time range out (past and future)
            {
                //Test Positive Time
                hashToTest = md5hex(string.Format("{0}{1}{2}", validAppKey.ToLower(), validSecret, tm + s));
                if (TokenToTest == hashToTest)
                    return true;

                hashToTest = md5hex(string.Format("{0}{1}{2}", validAppKey.ToLower(), validSecret, tm - s));
                if (TokenToTest == hashToTest)
                    return true;
                //return string.Format("Found valid token after {0} checkes, minus", s);
            }

            return false;
        }

        private static string md5hex(string str)
        {

            byte[] data = Encoding.Default.GetBytes(str);
            byte[] hash = _md5.ComputeHash(data);
            string hex = "";
            foreach (byte b in hash)
            {
                hex += String.Format("{0:x2}", b);
            }
            return hex;
        }
        public static string GenerateAppSecret()
        {
            return GenerateAppSecret(36);
        }
        public static string GenerateAppSecret(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}