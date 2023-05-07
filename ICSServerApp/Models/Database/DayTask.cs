namespace ICSServerApp.Models.Database;

public class DayTask
{
    public int Id { get; set; }
    public int PaintsNVarnishes { get; set; }
    public int Wood { get; set; }
    public string StartTime { get; set; } = String.Empty;
    public GoalType Type { get; set; }
}