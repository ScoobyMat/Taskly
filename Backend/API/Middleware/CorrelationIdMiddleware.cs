using Microsoft.Extensions.Primitives;

namespace API.Middleware;

public class CorrelationIdMiddleware
{
    private const string Header = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _log;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> log)
    { _next = next; _log = log; }

    public async Task Invoke(HttpContext ctx)
    {
        var cid = ctx.Request.Headers.TryGetValue(Header, out StringValues v) && !StringValues.IsNullOrEmpty(v)
            ? v.ToString() : Guid.NewGuid().ToString();

        ctx.Response.Headers[Header] = cid;
        using (_log.BeginScope(new Dictionary<string, object> { ["correlationId"] = cid }))
            await _next(ctx);
    }
}
