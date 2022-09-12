using Serilog;

namespace Microsoft.API.Infraestrutura.Filtros
{
    using Microsoft.ActionResults;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Hosting;
    using System.Net;

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger logger)
        {
            this._env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.Fatal(context.Exception, context.Exception.Message);

            var json = new JsonErrorResponse
            {
                Messages = new[] { "An error occur.Try it again." }
            };

            if (_env.IsDevelopment())
            {
                json.DeveloperMessage = context.Exception;
            }
           
            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMessage { get; set; }
        }
    }
}