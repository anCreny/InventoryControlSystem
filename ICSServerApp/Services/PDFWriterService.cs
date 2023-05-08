using ICSServerApp.Additionals;
using ICSServerApp.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ICSServerApp;

public class PDFWriterService
{
    private DatabaseContext _db;

    public PDFWriterService(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<PdfDocument> GenerateReport()
    {
        var finishedGoals = await _db.FinishedGoals.ToListAsync();

        var report = new PdfDocument();

        var page = report.AddPage();

        var graph = XGraphics.FromPdfPage(page);

        var obvFont = new XFont("Calibri", 14, XFontStyle.Regular);
        var headerFont = new XFont("Calibri", 16, XFontStyle.Bold);
        
        graph.DrawString("Отчет", headerFont, XBrushes.Black, new XRect(0,0, page.Width, page.Height), XStringFormats.TopCenter);
        foreach (var goal in finishedGoals)
        {
            var goalString = string.Empty;

            switch (goal.Type)
            {
                case GoalType.Input:
                    goalString = $"Была выполнена задача по загрузке товара на склад " +
                                 $"\nБыло загружено:" +
                                 $"{(goal.Wood > 0 ? $"{goal.Wood} поддонов с древисиной \n":"")}" +
                                 $"{(goal.PaintsNVarnishes > 0 ? $"{goal.PaintsNVarnishes} поддонов с лакокрасочными материалами" : "")} \n";
                    break;
                case GoalType.Output:
                    goalString = $"Была выполнена задача по выгрузке товара со склада " +
                                 $"\nБыло выгружено:" +
                                 $"{(goal.Wood > 0 ? $"{goal.Wood} поддонов с древисиной \n":"")}" +
                                 $"{(goal.PaintsNVarnishes > 0 ? $"{goal.PaintsNVarnishes} поддонов с лакокрасочными материалами" : "")} \n";
                    break;
            }
            
            graph.DrawString(goalString, obvFont, XBrushes.Black, new XRect(0,0, page.Width, page.Height), XStringFormats.CenterLeft);
        }

        string fileName = "Отчет_по_проделанной_работе_за_смену.pdf";
        report.Save(fileName);
        
        return report;
    }
}