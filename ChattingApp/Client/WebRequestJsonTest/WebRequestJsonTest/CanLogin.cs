using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebRequestJsonTest
{
    public class CanLogin
    {
        private const string END_POINT = "http://localhost:8080/login";
        public string userName { get; set; }
        public string userPassword { get; set; }

        public string Check()
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(END_POINT);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Cookie"] = Form1.CK;

            var json = new JObject();
            //json.Add("userName", userName);
            //json.Add("userPassword", userPassword);
            json.Add("user_id", userName);
            json.Add("user_pw", userPassword);

            string strJson = JsonConvert.SerializeObject(json);

            HttpWebResponse response = null;

            using(var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(strJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

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

        internal string Delete()
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/logout");

            request.Method = "GET";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

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
