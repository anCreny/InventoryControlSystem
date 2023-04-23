namespace ICSServerApp.Models.Database;

public class Goal
{
    public int Id { get; set; }
    public float PaintsNVarnishes { get; set; }
    public string Data { get; set; } = String.Empty;
    public string StartTime { get; set; } = String.Empty;
    public string EndTime { get; set; } = String.Empty;
    public float Wood { get; set; }
    public int StaffId { get; set; }
}