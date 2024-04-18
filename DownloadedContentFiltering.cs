﻿namespace PdfGetter;

public static class DownloadedContentFiltering
{
    // Method to remove files smaller than 500 KB from a directory
    public static void RemoveSmallFiles(string directoryPath)
    {
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            string pdfName;

            // Get all files in the directory
            FileInfo[] files = directoryInfo.GetFiles();

            // Filter files smaller than 500 KB
            IEnumerable<FileInfo> smallFiles = files.Where(file => file.Length < 500 * 1024);

            // Delete each small file
            foreach (FileInfo file in smallFiles)
            {
                pdfName = file.Name.Split('_')[1];
                pdfName = pdfName.Split('.')[0];

                Console.WriteLine($"File '{file.Name}' removed from '{directoryPath}' (size: {file.Length} bytes).");
                PdfDownloadStatusOverview.ReplaceDownloadInfoById(pdfName, "Not Downloaded", "No links working");
                file.Delete();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing small files: {ex.Message}");
        }
    }
}