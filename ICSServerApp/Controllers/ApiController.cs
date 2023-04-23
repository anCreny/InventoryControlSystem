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
    public async Task<IActionResult> Authorise(string login, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(user => user.Login == login && user.Password == password);
        
        if (user is not null)
        {
            HttpContext.Response.Cookies.Append("login", login);
            HttpContext.Response.Cookies.Append("password", password);
            HttpContext.Response.Cookies.Append("id", user.Id.ToString());
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

        var allUsers = _db.Users.Where(user => user.Id != 1).ToList();
        return Json(allUsers, options);
    }

    public async Task<IActionResult> GetGoal(int? id)
    {
        if (id is not null)
        {
            var goal = await _db.Goals.FirstOrDefaultAsync(goal => goal.Id == id);
            if (goal is null)
            {
                return NotFound();
            }

            return Json(goal);
        }

        var goals = await _db.Goals.ToListAsync();
        return Json(goals);
    }

    public async Task<IActionResult> AddGoal(Goal? goal)
    {
        if (goal is null)
        {
            return StatusCode(201);
        }

        await _db.Goals.AddAsync(goal);
        await _db.SaveChangesAsync();
        return Ok();
    }

    public async Task<IActionResult> UpdateGoal(Goal? goal)
    {
        if (goal is null)
        {
            return StatusCode(201);
        }

        _db.Goals.Update(goal);
        await _db.SaveChangesAsync();
        return Ok();
    }

    public async Task<IActionResult> DeleteGoal(int? id)
    {
        if (id is not null)
        {
            var goal = await _db.Goals.FirstOrDefaultAsync(goal => goal.Id == id);

            if (goal is not null)
            {
                _db.Goals.Remove(goal);
                await _db.SaveChangesAsync();
            }
        }

        return NotFound();
    }
}