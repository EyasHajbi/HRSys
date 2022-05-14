using Microsoft.AspNetCore.Http;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRSys.Api.Exception
{
    public class CustomSuccess : ICustomSuccess
    {
        public string GetSuccessResponse(string kMessage, string kSubmessage)
        {
            string _message = ExceptionMessage.ResourceManager.GetString(kMessage);
            string _subMessage = ExceptionMessage.ResourceManager.GetString(kSubmessage);

            string response = JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = _message,
                SubMessage = _subMessage
            }, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver() });

            return response;
        }
    }
}
