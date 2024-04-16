namespace PdfGetter;

public static class Utils
{
    public static bool FileExistsInDownloadedContent(string fileName)
    {
        string directoryPath = "downloadedContent";
        string filePath = Path.Combine(directoryPath, fileName);
        return File.Exists(filePath);
    }
}