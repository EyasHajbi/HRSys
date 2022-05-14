using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRSys.Api.Exception
{
    public class NotFoundCustomException : CustomException
    {
        public NotFoundCustomException(string message, string submessage)
            : base(HttpStatusCode.NotFound,message, submessage)
        {
        }
    }
}
