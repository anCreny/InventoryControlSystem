using System.Text.Json;
using ICSServerApp.Additionals;
using ICSServerApp.Additionals.Converters;
using ICSServerApp.Models;
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

    public async Task<IActionResult> MarkGoalDone(int? id)
    {
        if (id is not null)
        {
            var goal = await _db.Goals.FirstOrDefaultAsync(goal => goal.Id == id);
            
            if (goal is not null)
            {
                var links = _db.Links.ToList().Where(link => link.GoalId == goal.Id);
                var cellAddresses = new List<CellAddress>();
                foreach (var link in links)
                {
                    var cell = await _db.Cells.FirstOrDefaultAsync(cell => cell.Id == link.CellId);
                    cellAddresses.Add(new CellAddress{ Value = cell.Li + cell.Ni.ToString()});
                }
                
                var finishedGoal = new FinishedGoal
                {
                    Wood = goal.Wood,
                    PaintsNVarnishes = goal.PaintsNVarnishes,
                    Type = goal.Type,
                    StartTime = goal.StartTime,
                    StaffId = goal.StaffId,
                    CellAddresses = cellAddresses
                };

                _db.Goals.Remove(goal);
                _db.FinishedGoals.Add(finishedGoal);
                _db.Links.RemoveRange(links);

                await _db.SaveChangesAsync();
                
                return Ok();
            }
        }

        return NotFound();
    }

    public async Task<IActionResult> GetUserGoal(int? userid)
    {
        if (userid is not null)
        {
            var goal = await _db.Goals.FirstOrDefaultAsync(goal => goal.StaffId == userid);
            if (goal is not null)
            {
                var links = _db.Links.ToList().Where(link => link.GoalId == goal.Id);
                foreach (var link in links)
                {
                    var cell = await _db.Cells.FirstOrDefaultAsync(cell => cell.Id == link.CellId);
                    if (cell is not null)
                    {
                        goal.AccordedCells.Add(cell);
                    }
                }

                return Json(goal);
            }
        }

        return StatusCode(202);
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

            var links = _db.Links.ToList().Where(link => link.GoalId == goal.Id);
            foreach (var link in links)
            {
                var cell = await _db.Cells.FirstOrDefaultAsync(cell => cell.Id == link.CellId);
                if (cell is not null)
                {
                    goal.AccordedCells.Add(cell);
                }
            }

            return Json(goal);
        }

        var goals = await _db.Goals.ToListAsync();

        foreach (var goal in goals)
        {
            var links = _db.Links.ToList().Where(link => link.GoalId == goal.Id);
            foreach (var link in links)
            {
                var cell = await _db.Cells.FirstOrDefaultAsync(cell => cell.Id == link.CellId);
                if (cell is not null)
                {
                    goal.AccordedCells.Add(cell);
                }
            }
        }
        
        return Json(goals);
    }

    public async Task<IActionResult> GetLinks()
    {
        var links = await _db.Links.ToListAsync();
        return Json(links);
    }

    public async Task<IActionResult> AddGoal()
    {
        try
        {
            var goal = await HttpContext.Request.ReadFromJsonAsync<Goal>();
            if (goal is not null)
            {
                var newId = await _db.Goals.CountAsync() + 1;

                goal.Id = newId;
                var cells = new List<Cell>(goal.AccordedCells);
                var links = new List<CellsGoalLink>();
                foreach (var cell in cells)
                {
                    var link = new CellsGoalLink { GoalId = goal.Id, CellId = cell.Id};
                    links.Add(link);
                }

                _db.Links.AddRange(links);
                
                goal.AccordedCells = new();

                _db.Add(goal);

                await _db.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IActionResult> UpdateGoal()
    {
        try
        {
            var goal = await HttpContext.Request.ReadFromJsonAsync<Goal>();
            if (goal is not null)
            {
                var cells = new List<Cell>(goal.AccordedCells);
                var links = _db.Links.ToList().Where(link => link.GoalId == goal.Id);
                var linksForDelete = new List<CellsGoalLink>();
                foreach (var link in links)
                {
                    var deletable = true;
                    foreach (var cell in cells)
                    {
                        if (cell.Id == link.CellId)
                        {
                            deletable = false;
                        }
                    }

                    if (deletable)
                    {
                        linksForDelete.Add(link);
                    }
                }
                
                _db.Links.RemoveRange(linksForDelete);

                goal.AccordedCells = new();
                
                _db.Goals.Update(goal);
                await _db.SaveChangesAsync();
                return Ok();
            }

            return StatusCode(500);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

    public async Task<IActionResult> GetCell(string? address)
    {
        if (address is not null)
        {
            var li = address[0];
            var ni = 0;
            try
            {
                ni = Convert.ToInt32(address.Substring(1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
            Console.WriteLine($"[{li}{ni}]");
            var cell = await _db.Cells.FirstOrDefaultAsync(cell => cell.Li == li && cell.Ni == ni);
            if (cell is not null)
            {
                return Json(cell);
            }

            return NotFound();
        }

        var cells = await _db.Cells.ToListAsync();
        return Json(cells);
    }

    public async Task<IActionResult> UpdateCell()
    {
        try
        {
            var newCell = await HttpContext.Request.ReadFromJsonAsync<Cell>();
            if (newCell is not null)
            {
                _db.Cells.Update(newCell);
                await _db.SaveChangesAsync();
                return Ok();
            }

            return StatusCode(500);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IActionResult> AddDayTask(DayTask? dayTask)
    {
        if (dayTask is not null)
        {
            _db.DayTasks.Add(dayTask);
            await _db.SaveChangesAsync();
        }

        return Ok();
    }

    public async Task<IActionResult> GetDayTask(int? id)
    {
        if (id is not null)
        {
            var task = await _db.DayTasks.FirstOrDefaultAsync(task => task.Id == id.Value);
            if (task is null)
            {
                return NotFound();
            }

            return Json(task);
        }

        var tasks = await _db.DayTasks.ToListAsync();
        return Json(tasks);
    }

    public async Task UpdateDayTask()
    {
        try
        {
            var dayTask = await HttpContext.Request.ReadFromJsonAsync<DayTask>();
            if (dayTask is not null)
            {

                if (dayTask.Wood > 0 || dayTask.PaintsNVarnishes > 0)
                {
                    _db.DayTasks.Update(dayTask);
                    await _db.SaveChangesAsync();
                    return;
                }

                var dayTaskToDelete = await _db.DayTasks.FirstOrDefaultAsync(task => task.Id == dayTask.Id);
                if (dayTaskToDelete is not null)
                {
                    _db.DayTasks.Remove(dayTaskToDelete);
                    await _db.SaveChangesAsync();
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    public async Task GetReport()
    {
        await HttpContext.Response.SendFileAsync("wwwroot/Report.html");
    }

    public async Task<IActionResult> GetFinishedGoals()
    {
        var goals = await _db.FinishedGoals.ToListAsync();

        return Json(goals);
    }

    public async Task WipeDataBase()
    {
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();
    }
}