namespace PdfGetter;

public static class HttpRequestHandler
{
    public static async Task<int> HttpRequestStatus(string url, string fileName)
    {
        try
        {
            // Create the directory if it doesn't exist
            string directoryPath = "downloadedContent";
            Directory.CreateDirectory(directoryPath);

            // Combine the directory path and file name
            string filePath = Path.Combine(directoryPath, fileName);

            using (var client = new HttpClient())
            {
                // Set the user agent string
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.6367.61 Safari/537.36");

                using (var response = await client.GetAsync(url))
                {
                    var contentType = response.Content.Headers.ContentType;
                    int statusCode = (int)response.StatusCode;

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Failed to download {fileName} from {url}: {response.ReasonPhrase}");
                        Console.WriteLine();
                        return statusCode;
                    }

                    if (!contentType.MediaType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Failed to download {fileName} from {url}: not a pdf file");
                        Console.WriteLine();
                        return 404;
                    }

                    await using (var contentStream = await response.Content.ReadAsStreamAsync())
                    await using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await contentStream.CopyToAsync(fileStream, 81920); // Adjust buffer size as needed
                    }
                }
            }

            Console.WriteLine($"Downloaded {fileName} from {url}");
            Console.WriteLine();
            return 200; // Return the actual HTTP status code for success
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download {fileName} from {url}: {ex.Message}");
            Console.WriteLine();
            return 404; // Or another appropriate error status code
        }
    }
}