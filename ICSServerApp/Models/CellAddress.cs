using Microsoft.EntityFrameworkCore;

namespace ICSServerApp.Models;

[PrimaryKey(nameof(Id))]
public class CellAddress
{
    public int Id { get; set; }
    public string Value { get; set; } = String.Empty;
}