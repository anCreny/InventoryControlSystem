using ICSServerApp.Middlewares;

namespace ICSServerApp.Additionals;

public static class MiddlewaresHandler
{
    public static IApplicationBuilder UseCookieAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}