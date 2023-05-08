namespace ICSServerApp.Models.Database;

public class FinishedGoal
{
    public int Id { get; set; }
    public int PaintsNVarnishes { get; set; }
    public string StartTime { get; set; } = String.Empty;
    public int Wood { get; set; }
    public List<CellAddress> CellAddresses { get; set; } = new();
    public GoalType Type { get; set; }
    public int? StaffId { get; set; }
}