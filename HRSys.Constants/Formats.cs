using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Constants
{
    public static class Formats
    {
        public const string UkFormatForVM = "{0:dd/MM/yyyy}";
        public const string NumberFormat = @"^([0-9]*[1-9][0-9]*(\.[0-9]+)?|[0]+\.[0-9]*[1-9][0-9]*)$";
        public const string PositiveNumber = @"/^ (? !(?: 0 | 0\.0|0\.00)$)[+]?\d+(\.\d|\.\d[0 - 9])?$/ ";
        public const string DecimalPositiveNumber = @"^(\d*\.)?\d+$";
        public const string CharacterOnly = "([A-Za-z])";
        public const string UkFormatForString = "dd/MM/yyyy";
        public const string UkFormatWithTimeForString = "HH:mm dd/MM/yyyy";
        public const string NationalIdRegex = "^[0-9]{10}$";
        public const string MobileRegex = @"^(05)(0|1|2|3|4|5|6|7|8|9)([0-9]{7})+";
        public const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public const string FacebookRegex = @"http[s]?://(www|[a-zA-Z]{2}-[a-zA-Z]{2}|web|m|WEB|M)\.facebook\.com\/.+$";
        public const string TwitterRegex = @"^(https?\:\/\/)(twitter\.com)\/.+$";
        public const string InstagramRegex = @"^(https?\:\/\/)?(www\.)?(instagram\.com)\/.+$";
        public const string YoutubeRegex = @"^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?com)\/.+$";
        public const string NameRegex = @"^([A-z\u0600-\u06FF]?([ ]{0,}[A-z\u0600-\u06FF]+[A-z\u0600-\u06FF][ ]{0,})?[A-z\u0600-\u06FF][ ]{0,}){0,200}";//arabic and english with spaces
        public const string PasswordRegex = @"^([\w+\s+0-9]|[?=.*?[#?!@$%^&*-]){6,}";
        public const string OnlyNumberRegex = @"^[0-9]*$";
        public const string DatePickerFormat = @"dd/mm/yyyy";
    }
}
