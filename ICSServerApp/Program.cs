using ICSServerApp;
using ICSServerApp.Additionals;
using ICSServerApp.Additionals.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<UserHandlerService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var dbConnString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseMySql(dbConnString, new MySqlServerVersion(new Version(8, 0 , 32))));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapHub<DataHub>("/api/onlineData");

app.UseStaticFiles();

app.UseCookieAuthorization();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
