using ICSServerApp.Additionals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICSServerApp.Controllers;

public class HomeController : Controller
{
    private DatabaseContext _db;


    public HomeController(DatabaseContext db)
    {
        _db = db;
    }

    public async Task Index([FromServices] UserHandlerService userHandlerService)
    {
        var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == userHandlerService.Id);
        
        var accessRight = user.AccessRight;
        switch (accessRight)
        {
            case "admin":
                await HttpContext.Response.SendFileAsync("wwwroot/Home.html");
                break;
            case "loader":
                await HttpContext.Response.SendFileAsync("wwwroot/LoaderHomePage.html");
                break;
            case "operator":
                await HttpContext.Response.SendFileAsync("wwwroot/OperatorHomePage.html");
                break;
            default:
                await HttpContext.Response.WriteAsync("Cyka");
                break;
        }
    }

    public async Task Authorization()
    {
        await HttpContext.Response.SendFileAsync("wwwroot/AuthorizationPage.html");
    }
}
