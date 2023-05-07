using ICSServerApp.Additionals;
using Microsoft.EntityFrameworkCore;

namespace ICSServerApp.Middlewares;

public class AuthorizationMiddleware
{
    private RequestDelegate _next;
    private DatabaseContext _db;

    public AuthorizationMiddleware(RequestDelegate next, DatabaseContext db)
    {
        _next = next;
        _db = db;
    }

    public async Task Invoke(HttpContext context, UserHandlerService userHandlerService)
    {
        if (context.Request.Path.StartsWithSegments("/Api"))
        {
            await _next.Invoke(context);
        }
        else
        {
            var cookies = context.Request.Cookies;
            if (cookies.TryGetValue("login", out var login) && cookies.TryGetValue("password", out var password))
            {
                var user = await _db.Users.FirstOrDefaultAsync(user =>
                    user.Login == login && user.Password == password);
                
                if (user is not null)
                {
                    userHandlerService.SetId(user.Id);
                    Console.WriteLine($"{user.FullName}, {user.AccessRight}");
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.Cookies.Delete("login");
                    context.Response.Cookies.Delete("password");
                    context.Response.Cookies.Delete("id");
                    context.Response.Redirect("/", true);
                }
            }
            else
            {
                await context.Response.SendFileAsync("wwwroot/AuthorizationPage.html");
            }
        }
    }
}