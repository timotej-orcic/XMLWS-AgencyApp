using System;
using System.IO;
using System.Net;
using System.Xml;

namespace XML_WS_AgencyApp.Helpers
{
    public class SOAPHelper
    {
        public static string CallWebService(string url, string action, string payload)
        {
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(payload);
            HttpWebRequest webRequest = CreateWebRequest(url, action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                return soapResult;
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            string completeUri = url + "/" + action;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(completeUri);
            webRequest.Headers.Add(@"SOAP:Action" + completeUri);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/plain";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string payload)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();

            string xmlStr;
            if (payload == "")
            {
                xmlStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <soap:Body></soap:Body></soap:Envelope>";
            }
            else
            {
                xmlStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <soap:Body>"
                + payload +
                @"</root></soap:Body></soap:Envelope>";
            }

            soapEnvelopeDocument.LoadXml(xmlStr);
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}