using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace WebRequestJsonTest
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE,
    }

    public class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }

        public RestClient()
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.GET;
        }

        public string makeRequest()
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Method = httpMethod.ToString();
            request.Headers["Cookie"] = Form1.CK;

            String authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(userName + ":" + userPassword));
            //request.Headers.Add("Authorization", authHeader);

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (response.Headers["Set-Cookie"] != null)
                    Form1.CK = response.Headers["Set-Cookie"];

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("error code: " + response.StatusCode.ToString());
                }
                // Process the response stream... (could be JSON, XML or HTML etc...)

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        } // End of StreamReader
                    }
                }

                // End of using ResponseStream
            }
            catch (Exception ex)
            {
                strResponseValue = "{errorMessage: " + ex.Message.ToString();
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            return strResponseValue;
        }
    }
}
