using System.Net;
using Template.Application.Contracts.Services.Api;

namespace Template.API.Services {
    public class ClientContextService(IHttpContextAccessor httpContextAccessor) : IClientContextService {

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? GetClientIpAddress() {
            HttpContext? context = _httpContextAccessor.HttpContext;
            if (context is null)
                return null;
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor)) {
                var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (ips.Length > 0) {
                    var clientIp = ips[0].Trim();
                    if (IPAddress.TryParse(clientIp, out _)) {
                        return clientIp;
                    }
                }
            }

            // Check X-Real-IP header (alternative header used by some proxies)
            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp) && IPAddress.TryParse(realIp, out _)) {
                return realIp;
            }

            // Fallback to RemoteIpAddress
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        public bool IsWebClient() {
            var request = _httpContextAccessor.HttpContext?.Request;
            return request is not null && request.Headers.TryGetValue("X-Client-Type", out var headerValue) && headerValue == "Web";
        }
    }
}
