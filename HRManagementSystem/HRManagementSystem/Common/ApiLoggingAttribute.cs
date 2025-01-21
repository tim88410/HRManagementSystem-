using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace HRManagementSystem.Common
{
    public class ApiLoggingAttribute : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var originalResponseBodyStream = context.HttpContext.Response.Body;
            using (var responseBodyStream = new MemoryStream())
            {
                context.HttpContext.Response.Body = responseBodyStream;

                await next();

                context.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(context.HttpContext.Response.Body).ReadToEndAsync();
                Log.Information("Request: {Method} {Path} with body {Body} from IP {IpAddress}",
                    context.HttpContext.Request.Method,
                    context.HttpContext.Request.Path,
                    responseBody,
                    context.HttpContext.Connection.RemoteIpAddress);

                context.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            }
        }
    }
}
