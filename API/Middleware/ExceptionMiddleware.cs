using Application.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate request;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment ihv;

        public ExceptionMiddleware(RequestDelegate request,ILogger<ExceptionMiddleware> logger,IHostEnvironment ihv)
        {
            this.request = request;
            this.logger = logger;
            this.ihv = ihv;
        }


        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                 await request(httpcontext);
            }

            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                httpcontext.Response.ContentType = "application/json";
                httpcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = ihv.IsDevelopment()

                    ? new AppException(httpcontext.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())

                    : new AppException(httpcontext.Response.StatusCode, "Server Error");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response, options);

                await httpcontext.Response.WriteAsync(json);
            }
        }
    }
}
