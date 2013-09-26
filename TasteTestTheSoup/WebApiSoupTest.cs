using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TasteTestTheSoup
{
    [TestClass]
    public class WebApiSoupTest
    {

        private String firstAppId = "a75a9377-4e07-4582-9808-0e29478477a0";
        private String firstAppSecret = "EIbl7vLiCHphVhUJDDh78c7scVPLwJNnM00J";

        [TestMethod]
        public void TestHandShake()
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:58720/");
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:58720/api/SoupRecipes/PerformHandshake"));
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                msg.Headers.Add("AppId", firstAppId);
                msg.Headers.Add("AppToken", CreateToken(firstAppId, firstAppSecret));

                HttpResponseMessage response = client.SendAsync(msg).Result;
                string responseAsString = response.Content.ReadAsStringAsync().Result;
                if(response.StatusCode != System.Net.HttpStatusCode.OK)
                    Assert.Fail(String.Format("Failed with code: {0}", response.StatusCode));
            }
            catch (Exception ex)
            {
                Assert.Fail(String.Format("Exception Occured: {0}", ex.Message));
            }


        }




        private string CreateToken(string applicationId, string secret)
        {


            string retVal = string.Empty;
            string tm = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            string toEncrypt = string.Format("{0}{1}{2}", applicationId, secret, tm);
            try
            {
                return md5hex(toEncrypt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string md5hex(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(str);
            byte[] hash = md5.ComputeHash(data);
            string hex = "";
            foreach (byte b in hash)
            {
                hex += String.Format("{0:x2}", b);
            }
            return hex;
        }
    }
}
