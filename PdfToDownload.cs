namespace PdfGetter;

public static class PdfToDownload
{
    public static async Task DownloadPdfOrHtmlAsync(List<string?> pdfListIds, List<string?> pdfListLinks1, List<string?> pdfListLinks2)
    {
        var tasks = new List<Task<(string, int, string)>>();

        for (int i = 0; i < pdfListIds.Count; i++)
        {
            tasks.Add(ProcessFileAsync(pdfListIds[i], pdfListLinks1[i], pdfListLinks2[i]));
        }

        var downloadStatus = await Task.WhenAll(tasks);

        // Generate report
        PdfDownloadStatusOverview.GenerateReport(downloadStatus.ToList());
    }

    private static async Task<(string, int, string)> ProcessFileAsync(string id, string link1, string link2)
    {
        string usedLink = "";
        int downloadSuccess = 404;

        // File name
        string fileName = $"File_{id}.pdf";

        // Check if the file already exists in the downloadedContent folder
        if (Utils.FileExistsInDownloadedContent(fileName))
        {
            Console.WriteLine($"File '{fileName}' already exists in downloadedContent. Skipping download.");
            downloadSuccess = 200; // Successfully "downloaded"
        }
        else
        {
            // Attempt to download from the primary link (pdfListLinks1)
            if (!string.IsNullOrWhiteSpace(link1))
            {
                usedLink = "Link1";
                downloadSuccess = await HttpRequestHandler.HttpRequestStatus(link1, fileName);

                if (downloadSuccess != 200)
                    usedLink = "No links working";
            }
            else if (!string.IsNullOrWhiteSpace(link2))
            {
                // If link1 is empty, try the secondary link (pdfListLinks2)
                usedLink = "Link2";
                downloadSuccess = await HttpRequestHandler.HttpRequestStatus(link2, fileName);

                if (downloadSuccess != 200)
                    usedLink = "No links working";
            }
            else
            {
                usedLink = "No links working";
            }
        }

        return (id, downloadSuccess, usedLink);
    }

}
