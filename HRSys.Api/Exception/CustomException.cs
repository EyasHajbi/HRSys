using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRSys.Api.Exception
{
    public class CustomException : System.Exception
    {
        private HttpStatusCode _code;
        private string _submessage;

        public HttpStatusCode Code
        {
            get => _code;
        }
        public string SubMessage
        {
            get => _submessage;
        }

        public CustomException(HttpStatusCode code, string message, string submessage) : base(message)
        {
            
            _code = code;
            _submessage = submessage;
        }

       

    }
}
