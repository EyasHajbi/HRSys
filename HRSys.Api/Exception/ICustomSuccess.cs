using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRSys.Api.Exception
{
    public interface ICustomSuccess
    {
        public string GetSuccessResponse(string message, string submessage);

    }
}
