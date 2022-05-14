using HRSys.DTO.App_Users;
using HRSys.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Api.Extensions
{
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        public static async Task<dynamic> GetRawBodyStringAsync<T>(this HttpRequest request, Encoding encoding = null)
        {
            string json = "";

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                json = await reader.ReadToEndAsync();

            string result = json.AndroidDecrypt();

            var val = JsonConvert.DeserializeObject<T>(result);
            return val;

            //result;
        }


        public static async Task<string> GetEncryptRawBodyAsync(this HttpRequest request, Encoding encoding = null)
        {
            string json = "";

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                json = await reader.ReadToEndAsync();

            string result = json.AndroidEncrypt();

            return result;
            //return JsonConvert.DeserializeObject<T>(result);

            //result;
        }

        public static async Task<string> GetDecryptRawBodyAsync(this HttpRequest request, Encoding encoding = null)
        {
            string json = "";

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                json = await reader.ReadToEndAsync();

            string result = json.AndroidDecrypt();

            return result;
            //return JsonConvert.DeserializeObject<T>(result);

            //result;
        }
        public static string SaveBase64(this string Base64ImageString, string Dir, string FileName, string FileType)
        {
            try
            {
                var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                Dir = string.Format("Uploaded/{0}", Dir);
                var path = string.Format("{0}\\{1}", root, Dir.Replace("/", "\\"));
                path = Path.Combine(Directory.GetCurrentDirectory(), path);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + "\\" + FileName + "." + FileType;
                File.WriteAllBytes(filePath, Convert.FromBase64String(Base64ImageString.Replace("data:image/jpeg;base64,", "")));
                return $"~/Uploaded/{Dir}/{FileName +"."+ FileType}";
            }
            catch (System.Exception ex)
            {
                return "";
            }

        }


    }
}
