namespace PdfGetter;

public static class PdfDownloadStatusOverview
{
    public static void GenerateReport(List<(string, int, string)> downloadStatus)
    {
        using (StreamWriter writer = new StreamWriter("DownloadReport.csv"))
        {
            writer.WriteLine("ID,DownloadStatus,UsedLink");
            foreach (var item in downloadStatus)
            {
                string downloadStatusText = item.Item2 == 200 ? "Downloaded" : "Not Downloaded"; // Interpret HTTP status code
                string usedLink = string.IsNullOrWhiteSpace(item.Item3) ? "No links working" : item.Item3;

                writer.WriteLine($"{item.Item1},{downloadStatusText},{usedLink}");
            }
        }

        Console.WriteLine("Download report generated successfully.");
    }
}