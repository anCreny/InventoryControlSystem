namespace ICSServerApp.Models.Database;

public class Cell
{
    public int Id { get; set; }
    public char Li { get; set; } // Letter Index of the cell
    public int Ni { get; set; } // Number Index of the cell
    public CellType Type { get; set; }
    public ProductType? ProductType { get; set; }

    public CellStatus CellStatus { get; set; }
}