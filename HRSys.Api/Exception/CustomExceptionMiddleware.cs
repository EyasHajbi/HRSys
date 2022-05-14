using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRSys.Api.Exception
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context,System.Exception exception)
        {
            var response = context.Response;
            var customException = exception as CustomException;
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Unexpected server error";
            var submessage = exception.Message;

            if (null != customException)
            {
                statusCode = customException.Code;
                message = customException.Message;
                submessage = customException.SubMessage;            
            }

            response.ContentType = "application/json;charset=utf-8";
            response.StatusCode = (int)statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = message,
                SubMessage = submessage
            }));
        }
    }
}
