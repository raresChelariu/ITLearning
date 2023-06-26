using System.Diagnostics;
using System.Text;

namespace ITLearningAPI.Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? string.Empty;
        var spanId = Activity.Current?.SpanId.ToString() ?? string.Empty;

        _logger.LogInformation("{@Request}", await RequestAsLogMessage(context.Request, traceId, spanId));

        await _next.Invoke(context);

        _logger.LogInformation("{@Response}", ResponseAsLogMessage(context.Response, traceId, spanId));
    }

    private static string ResponseAsLogMessage(HttpResponse response, string traceId, string spanId)
    {
        var message = new StringBuilder();
        message.AppendLine($"Trace Id: {traceId}");
        message.AppendLine($"Span Id: {spanId}");
        message.AppendLine($"Response HTTP Status Code: {response.StatusCode}");
        message.AppendLine($"Response Headers: {GetHeadersAsString(response.Headers)}");

        return message.ToString();
    }

    private static async Task<string> RequestAsLogMessage(HttpRequest request, string traceId, string spanId)
    {
        var message = new StringBuilder();
        var requestPath = GetPath(request);
        message.AppendLine($"Trace Id: {traceId}");
        message.AppendLine($"Span Id: {spanId}");
        message.AppendLine($"Request Url: {requestPath}");
        message.AppendLine($"Request HTTP Verb: {request.Method}");
        message.AppendLine($"Request Headers: {Environment.NewLine} {GetHeadersAsString(request.Headers)}");
        message.AppendLine("Request Body: ");
        if (!requestPath.Contains("video"))
        {
            message.AppendLine($"{await GetBody(request)}{Environment.NewLine}");    
        }
        return message.ToString();
    }

    private static async Task<string> GetBody(HttpRequest request)
    {
        request.EnableBuffering();
        var requestBodyStream = new StreamReader(request.Body);
        var body = await requestBodyStream.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static string GetHeadersAsString(IHeaderDictionary headers)
    {
        return headers
            .Aggregate(new StringBuilder(),
                (r, x) => r.AppendLine($"{x.Key} = {x.Value.ToString()}"))
            .ToString();
    }

    private static string GetPath(HttpRequest request)
    {
        var parts = new []
        {
            request.Scheme, "://", 
            request.Host.ToUriComponent(), 
            request.PathBase.ToUriComponent(), 
            request.Path.ToUriComponent(), 
            request.QueryString.ToUriComponent()
        };

        return string.Concat(parts);
    }
}