using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http) => _http = http;

    public Guid UserId
    {
        get
        {
            var user = _http.HttpContext?.User;
            var val = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? user?.FindFirst("sub")?.Value;

            return Guid.TryParse(val, out var id) ? id : Guid.Empty;
        }
    }

    public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;
}
