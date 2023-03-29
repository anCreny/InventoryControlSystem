namespace ICSServerApp.Models.Database;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string AccessRight { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}