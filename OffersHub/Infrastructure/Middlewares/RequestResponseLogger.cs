using System.Diagnostics;

namespace OffersHub.API.Infrastructure.Middlewares
{
    public class RequestResponseLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLogger> _logger;

        public RequestResponseLogger(RequestDelegate next, ILogger<RequestResponseLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await LogRequest(context.Request);

                // Capture the original response body stream
                var originalBodyStream = context.Response.Body;
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream; // Redirect response to memory stream

                await _next(context);

                stopwatch.Stop();
                await LogResponse(context.Response, responseBodyStream);

                // Reset response body to original stream
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception caught in middleware: {ex}");
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"Request completed in {stopwatch.ElapsedMilliseconds}ms");
            }
            //await LogRequest(context.Request);
            //await _next(context);
            //await LogResponse(context.Response);
        }

        private async Task LogRequest(HttpRequest request)
        {
            request.EnableBuffering(); // Allows re-reading the body
            string body = string.Empty;

            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                request.Body.Position = 0; // Reset position for further processing
            }

            var log = $"[Request] {DateTime.Now}{Environment.NewLine}" +
                      $"IP={request.HttpContext.Connection.RemoteIpAddress}{Environment.NewLine}" +
                      $"Method={request.Method}{Environment.NewLine}" +
                      $"Path={request.Path}{Environment.NewLine}" +
                      $"IsSecured={request.IsHttps}{Environment.NewLine}" +
                      $"QueryString={request.QueryString}{Environment.NewLine}" +
                      $"Body={body}{Environment.NewLine}";

            _logger.LogInformation(log);
        }

        private async Task LogResponse(HttpResponse response, MemoryStream responseBodyStream)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(responseBodyStream).ReadToEndAsync();

            var log = $"[Response] {DateTime.Now}{Environment.NewLine}" +
                      $"Status Code={response.StatusCode}{Environment.NewLine}" +
                      $"Body={body}{Environment.NewLine}";

            _logger.LogInformation(log);
        }
    }
}
