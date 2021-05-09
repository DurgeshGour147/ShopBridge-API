using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry_Store.Common.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _performanceLogger = null;

        private readonly ILogger _exceptionLogger;
        public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _performanceLogger = loggerFactory.CreateLogger("Performance");
            _exceptionLogger = loggerFactory.CreateLogger("Debug");
        }

        public async Task Invoke(HttpContext context)
        {
            RequestDelegate tempnext = _next;
            HttpContext tempcontext = context;

            string request = string.Empty;

            //Start the timer
            Stopwatch sw = Stopwatch.StartNew();

            //First, get the incoming request
            request = await FormatRequest(context.Request);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;
                try
                {
                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);
                }

                catch (Exception ex)
                {
                    if (request.Contains("/File/Upload"))
                    {
                        int index = request.IndexOf("Content-Disposition");
                        if (index > 0)
                            request = request.Remove(index);
                    }
                    _exceptionLogger.LogError(DateTime.Now + $" Application exception: {ex}" + Environment.NewLine + $"For Request: {request}");
                    await HandleExceptionAsync(context, ex);
                }
                //Format the response from the server
                var response = await FormatResponse(context.Response);
                // Save log into log4net
                if (sw != null)
                    sw.Stop();
                if (_performanceLogger != null)
                    LogPerformanceAspect(request, _performanceLogger, sw);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body.Position = 0;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }

        private void LogPerformanceAspect(string request, ILogger logger, Stopwatch sw)
        {
            string output = string.Format(DateTime.Now + " Request Executed in {0} seconds", sw.ElapsedMilliseconds / 1000);
            if (sw.ElapsedMilliseconds / 1000 > 3)
                if (!request.Contains("/File/Upload"))
                    logger.LogWarning(output + Environment.NewLine + request);
                else
                    logger.LogWarning(output + Environment.NewLine + request.Remove(request.IndexOf("Content-Disposition")));
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = exception.IsNotNull() && exception is UnauthorizedAccessException ? 401 : 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(ExtensionMethod.SerializeObject(new ErrorDetails()
            {
                ErrorMessage = "Internal_Server_Error_from_the_custom_middleware",
                StackTrace = exception.Message + "---" + (ConfigManager.Instance.IsStaging ? exception.StackTrace : string.Empty),
                ErrorDescription = exception.Message
            }), Encoding.UTF8);
        }
    }

    public class ErrorDetails
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string ErrorDescription { get; set; }
    }
}
