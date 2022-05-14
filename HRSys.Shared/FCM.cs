using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace HRSys.Shared
{
    public class FCM
    {
        #region FCM properties

        private const string ApplicationID = "AAAAqekmhBI:APA91bH6c7P6ydbRJk8R-wjuBvRI9qB4uV8zYxPLZHOs-wauvjs7pbYYnHx_yhhb8Wca8JD_9NTirnHmyAO9hFcsM2dmlsWXGKOazofG1BeXrEiM4L7XMRU03A3NStJQYwH8nIG76clF";
        // "AAAAyVEZsjo:APA91bG7sDMpiTi95RoUGqc81wbNQlVTVvNT_xAN9rsjDVsv__c_WEIxFEVbeXuxqjThws3b8qJE7id4hwlxDe_lTMq_lTHCtW3Im_l48V11jYwfNXJoZGRF86BG4kNevZ7MB16meKmH";
        private const string SenderID = "729761088530";//"864649065018";

        public string DeviceID { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }

        public object AdditionalData { get; set; }
        #endregion

        #region FCM Methods               
        public async Task<bool> Send()
        {
            try
            {


                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var objDocument = new
                {
                    to = DeviceID,
                    notification = new
                    {
                        title = Subject,
                        body = Message,
                        content_available = true,
                        sound = "Default"
                    },
                    data = AdditionalData
                };
                string jsonDocumentFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objDocument);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonDocumentFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ApplicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SenderID));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();

                            }
                        }

                    }
                }

                return    true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion

    }


    //    FCM fcm = new FCM();
    //    fcm.DeviceID = "/topics/test";
    //fcm.Subject = modelVM.NotificationTitleAr;
    //fcm.Message = modelVM.NotificationBodyAr;
    //fcm.AdditionalData = additionalData;
    //fcm.Send();

}
