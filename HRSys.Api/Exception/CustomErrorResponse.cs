using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRSys.Api.Exception
{
    public class CustomErrorResponse
    {
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("SubMessage")]
        public string SubMessage { get; set; }
    }
 
}
