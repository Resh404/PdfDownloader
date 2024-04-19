namespace PdfGetter;

public static class PdfToDownload
{
    public static async Task DownloadPdfAsync(List<string?> pdfListIds, List<string?> pdfListLinks1, List<string?> pdfListLinks2)
    {
        int batchSize = 50; // batch size 25 = 7750 pdf files very long processing time, batch size 100 = 6486 long
        // batch size 1000 = 1624 short, batch size 10000 = 295 -> 503 -> 594 (multi runs) very short

        int totalFiles = pdfListIds.Count; // Get the total number of files to process
        int processedFiles = 0; // Initialize the processed files counter

        var tasks = new List<Task<(string, int, string)>>();
        var semaphore = new SemaphoreSlim(batchSize, batchSize); // Create a semaphore to control the number of concurrent tasks

        for (int i = 0; i < totalFiles; i++)
        {
            await semaphore.WaitAsync(); // Acquire a semaphore slot
            tasks.Add(ProcessFileAsync(pdfListIds[i], pdfListLinks1[i], pdfListLinks2[i])
                .ContinueWith(t =>
                {
                    semaphore.Release(); // Release the semaphore slot when the task is completed
                    Interlocked.Increment(ref processedFiles); // Increment the processed files counter atomically multi thread safe
                    Console.WriteLine($"Processed files: {processedFiles} out of {totalFiles}"); // Display the progress
                    return t.Result;
                }));
        }

        var downloadStatus = await Task.WhenAll(tasks);

        // Generate report
        PdfDownloadStatusOverview.GenerateReport(downloadStatus.ToList());
    }

    private static async Task<(string, int, string)> ProcessFileAsync(string id, string link1, string link2)
    {
        string usedLink;
        int downloadSuccess = 404;

        // File name
        string fileName = $"File_{id}.pdf";
        string filePath = Path.Combine("./DownloadedContent", fileName);

        // Check if the file already exists in the DownloadedContent folder
        if (File.Exists(filePath))
        {
            Console.WriteLine($"File '{fileName}' already exists in DownloadedContent. Skipping download.");
            Console.WriteLine();
            downloadSuccess = 200; // Successfully "Downloaded"
            usedLink = string.IsNullOrWhiteSpace(link1) ? link2 : link1;
        }
        else
        {
            // Attempt to download from the primary link
            if (!string.IsNullOrWhiteSpace(link1))
            {
                usedLink = "Link1";
                downloadSuccess = await HttpRequestHandler.HttpRequestStatus(link1, fileName);

                if (downloadSuccess != 200)
                    usedLink = "No links working";
            }
            else if (!string.IsNullOrWhiteSpace(link2))
            {
                // If link1 is empty, try the secondary link
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
