using System.Text.Json;
using ICSServerApp.Additionals;
using ICSServerApp.Additionals.Converters;
using ICSServerApp.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICSServerApp.Controllers;

public class ApiController : Controller
{
    private DatabaseContext _db;
    
    public ApiController(DatabaseContext db)
    {
        _db = db;
    }
    
    public void Index()
    {
        
    }

    public async Task<IActionResult> DeleteUser(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == id);
        
        if (user is not null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        return Ok();
    }
    
    public async Task<IActionResult> UpdateUser(int? id, string? fullname, string? login, string? password, string? accessRight)
    {
        if (id is null || fullname is null || login is null || password is null || accessRight is null)
        {
            return BadRequest();
        }

        var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == id);
        user.FullName = fullname;
        user.Login = login;
        user.AccessRight = accessRight;
        user.Password = password;

        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return Ok();
    }

    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("login");
        HttpContext.Response.Cookies.Delete("password");

        return Ok();
    }
    
    [method: HttpPost]
    public IActionResult Authorise(string login, string password)
    {
        var users = _db.Users.Where(user => user.Login == login && user.Password == password).ToList();
        if (users.Count > 0)
        {
            HttpContext.Response.Cookies.Append("login", login);
            HttpContext.Response.Cookies.Append("password", password);
            return Ok();
        }

        return NotFound();
    }

    public async Task<IActionResult> CreateUser(string? fullname, string? login, string? password, string? accessRight)
    {
        if (fullname is null || login is null || password is null || accessRight is null)
        {
            return BadRequest();
        }

        var user = new User();
        user.FullName = fullname;
        user.Login = login;
        user.Password = password;
        user.AccessRight = accessRight;

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        return Ok();
    }


    public async Task AddUser()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UserConverter());

        var user = await HttpContext.Request.ReadFromJsonAsync<User>(options);
        if (user is not null)
        {
            _db.Users.Add(user);
        }
        await _db.SaveChangesAsync();
    }

    public async Task<IActionResult> GetUser(int? id)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UserConverter());
        
        if (id is not null)
        {
            var users = _db.Users.Where(user => user.Id == id).ToList();
            if (users.Count > 0)
            {
                var user = users[0];

                return Json(user, options);
            }
            return NotFound();
        }

        var allUsers = _db.Users.ToList();
        return Json(allUsers, options);
    }
}