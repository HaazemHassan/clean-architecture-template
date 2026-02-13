namespace Template.API.Middlewares {

    public static class MiddlewareExtensions {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder) {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }

        public static IApplicationBuilder UseGuestSession(this IApplicationBuilder builder) {
            return builder.UseMiddleware<GuestSessionMiddleware>();
        }
    }
}
