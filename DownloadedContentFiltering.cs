namespace PdfGetter;

public static class DownloadedContentFiltering
{
    // Method to remove files smaller than 500 KB from a directory
    public static void RemoveEmptyPdfFiles(string directoryPath)
    {
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            // Get all files in the directory
            FileInfo[] files = directoryInfo.GetFiles();

            // Filter files smaller than 6 KB (largest empty PDF file is 5 KB)
            IEnumerable<FileInfo> smallFiles = files.Where(file => file.Length < 6 * 1024);

            // Delete each small file
            foreach (FileInfo file in smallFiles)
            {
                string pdfName = file.Name.Split('_')[1];
                pdfName = pdfName.Split('.')[0];

                Console.WriteLine($"File '{file.Name}' removed from '{directoryPath}' reason: empty PDF(size: {file.Length} bytes).");
                Console.WriteLine();
                PdfDownloadStatusOverview.ReplaceDownloadInfoById(pdfName, "Not Downloaded", "No links working");
                file.Delete();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing empty PDF files: {ex.Message}");
        }
    }
}
