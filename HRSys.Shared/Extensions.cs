using HRSys.Enum;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRSys.Shared
{
    public static class Extensions
    {
        private static Lang GetCurrentLang()
        {
            string code = Thread.CurrentThread.CurrentCulture.Parent.Name;
            try
            {
                Lang lang = (Lang)HRSys.Enum.Lang.Parse(typeof(Lang), code);
                return lang;
            }
            catch
            {
                return Lang.ar;
            }
        }


        public static string To12Hours(this TimeSpan timeSpan)
        {

            Lang lang = GetCurrentLang();

            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var amPmDesignator = (lang == Lang.ar ? "ص" : "AM");
            if (hours == 0)
                hours = 12;
            else if (hours == 12)
                amPmDesignator = (lang == Lang.ar ? "م" : "PM");
            else if (hours > 12)
            {
                hours -= 12;
                amPmDesignator = (lang == Lang.ar ? "م" : "PM");
            }
            var formattedTime = string.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);

            return formattedTime;
        }

        public static string To12Hours(this TimeSpan? timeSpan)
        {
            if (!timeSpan.HasValue)
            { return string.Empty; }

            Lang lang = GetCurrentLang();

            var hours = timeSpan.Value.Hours;
            var minutes = timeSpan.Value.Minutes;
            var amPmDesignator = (lang == Lang.ar ? "ص" : "AM");
            if (hours == 0)
                hours = 12;
            else if (hours == 12)
                amPmDesignator = (lang == Lang.ar ? "م" : "PM");
            else if (hours > 12)
            {
                hours -= 12;
                amPmDesignator = (lang == Lang.ar ? "م" : "PM");
            }
            var formattedTime = string.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);

            return formattedTime;
        }



        #region Encrypt/Decrypt between Java and .NET methods

        private static string secretKey = "HRSysY2020HRSysY2020";
        private static object encoding;

        public static RijndaelManaged GetRijndaelManaged()
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }
        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
 

        public static string AndroidEncrypt(this string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged()));
            //return plainText;
        }
        public static string AndroidDecrypt(this string cipherText)
        {
             var encryptedBytes = Convert.FromBase64String(cipherText);
             return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged()));
            //return cipherText;
        }
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion


       

    }
}
