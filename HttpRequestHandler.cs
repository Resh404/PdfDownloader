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
            using (var response = await client.GetAsync(url))
            {
                int statusCode = (int)response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to download {fileName} from {url}: {response.ReasonPhrase}");
                    return statusCode;
                }

                await using (var contentStream = await response.Content.ReadAsStreamAsync())
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await contentStream.CopyToAsync(fileStream, 81920); // Adjust buffer size as needed
                }
            }
            Console.WriteLine($"Downloaded {fileName} from {url}");
            return 200; // Return the actual HTTP status code for success
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download {fileName} from {url}: {ex.Message}");
            return 404; // Or another appropriate error status code
        }
    }
}