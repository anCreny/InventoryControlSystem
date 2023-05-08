using ICSServerApp.Additionals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Pdf;

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
                await HttpContext.Response.SendFileAsync("wwwroot/AdminPage.html");
                break;
            case "loader":
                await HttpContext.Response.SendFileAsync("wwwroot/LoaderHomePage.html");
                break;
            case "operator":
                await HttpContext.Response.SendFileAsync("wwwroot/OperatorHomePage.html");
                break;
        }
    }

    public async Task GetReport()
    {
        
    }

    public async Task Authorization()
    {
        await HttpContext.Response.SendFileAsync("wwwroot/AuthorizationPage.html");
    }
}
