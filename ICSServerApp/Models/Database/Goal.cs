using Microsoft.EntityFrameworkCore;

namespace ICSServerApp.Models.Database;

[PrimaryKey(nameof(Id))]
public class Goal
{
    public int Id { get; set; }
    public int PaintsNVarnishes { get; set; }
    public string StartTime { get; set; } = String.Empty;
    public int Wood { get; set; }
    public List<Cell> AccordedCells { get; set; } = new();
    public GoalType Type { get; set; }
    public int? StaffId { get; set; }
}