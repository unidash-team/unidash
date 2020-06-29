using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Unidash.Core.Utilities
{
    public class HttpContextCurrentUserAccessor : ICurrentUserAccessor, IUserIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId() => GetUserIdFromPrincipal(_httpContextAccessor.HttpContext.User);

        public string GetUserId(HubConnectionContext connection) => GetUserIdFromPrincipal(connection.User);

        public static string GetUserIdFromPrincipal(ClaimsPrincipal user) => user.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}