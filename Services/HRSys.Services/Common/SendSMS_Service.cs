using HRSys.DTO;
using HRSys.Enum;
using HRSys.Services.Common.Interface;
using HRSys.Services.SystemSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Common
{
    public class SendSMS_Service :ISendSMS_Service
    {


        private class TokenResponse
        {
            private string m_status;
            public string status
            {
                get
                {
                    return m_status;
                }
                set
                {
                    m_status = value;
                }
            }
            private TokenResult m_result;
            public TokenResult result
            {
                get
                {
                    return m_result;
                }
                set
                {
                    m_result = value;
                }
            }
        }

        private class TokenResult
        {
            private string m_integration_token;
            public string integration_token
            {
                get
                {
                    return m_integration_token;
                }
                set
                {
                    m_integration_token = value;
                }
            }
            private string m_accountSid;
            public string accountSid
            {
                get
                {
                    return m_accountSid;
                }
                set
                {
                    m_accountSid = value;
                }
            }
        }
        public void  SendSMS(string Mobile ,string Message)
        {
            

            string AuthToken = "";
            try
            {
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://zsms.jo.zain.com/core/user/rest/user/generateintegrationtoken");
                tRequest.Method = "POST";
                tRequest.ContentType = " application/json;charset=UTF-8";
                // http headers
                tRequest.Headers.Add(string.Format("username: {0}", "911"));
                tRequest.Headers.Add(string.Format("password: {0}", "Aa@123456"));
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes("");
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                // WebResponse tResponse = InlineAssignHelper(ref tResponse, tRequest.GetResponse());
                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                string sResponseFromServer = tReader.ReadToEnd();
                TokenResponse _response = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(sResponseFromServer);
                AuthToken = _response.result.integration_token;
                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch (Exception ex)
            {
                //BusinessObject.LogError("SMS Token API", ex.Message);
            }
            try
            {
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://zsms.jo.zain.com/core/corpsms/sendNow");
                tRequest.Method = "POST";
                tRequest.ContentType = " application/json;charset=UTF-8";
                // http headers
                tRequest.Headers.Add(string.Format("Authorization: {0}", AuthToken));
                tRequest.Headers.Add(string.Format("integration_token: {0}", AuthToken));
                Dictionary<string, object> postData = new Dictionary<string, object>();
                List<string> _mobilelst = new List<string>();
                _mobilelst.Add(Mobile);
                postData.Add("phone_numbers", _mobilelst);

                postData.Add("content", "" + Message + "");
                postData.Add("sender_id", "911");
                postData.Add("service_type", "bulk_sms");
                postData.Add("recipient_numbers_type", "single_numbers");
                string myJsonString = (new JavaScriptSerializer()).Serialize(postData);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(myJsonString);
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                //WebResponse tResponse = InlineAssignHelper(ref tResponse, tRequest.GetResponse());
                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                string sResponseFromServer = tReader.ReadToEnd();
                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch (Exception ex)
            {
                //BusinessObject.LogError("SMS API", ex.Message);
            }
        }







    }

}
