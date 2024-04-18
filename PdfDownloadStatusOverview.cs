using Newtonsoft.Json;

namespace PdfGetter
{
    public static class PdfDownloadStatusOverview
    {
        public static void GenerateReport(List<(string, int, string)> downloadStatus)
        {
            List<object> records = [];

            foreach (var item in downloadStatus)
            {
                string downloadStatusText = item.Item2 == 200 ? "Downloaded" : "Not Downloaded";
                string usedLink = string.IsNullOrWhiteSpace(item.Item3) ? "No links working" : item.Item3;

                var record = new
                {
                    ID = item.Item1,
                    DownloadStatus = downloadStatusText,
                    UsedLink = usedLink
                };

                records.Add(record);
            }

            string json = JsonConvert.SerializeObject(records, Formatting.Indented);
            File.WriteAllText("DownloadReport.json", json);

            Console.WriteLine("Download report generated successfully.");
        }

        public static void ReplaceDownloadInfoById(string idToReplace, string newDownloadStatus, string newUsedLink)
        {
            try
            {
                string downloadStatusReportPath = "./DownloadReport.json";

                // Read the JSON file into a list of records
                List<DownloadRecord> records = JsonConvert.DeserializeObject<List<DownloadRecord>>(File.ReadAllText(downloadStatusReportPath));

                // Find the record with the specified ID
                var recordToUpdate = records.FirstOrDefault(record =>
                {
                    Console.WriteLine($"Comparing ID: '{record.ID}' with '{idToReplace}'");
                    return string.Equals(record.ID, idToReplace.Trim(), StringComparison.CurrentCultureIgnoreCase);
                });

                // Print the content of recordToUpdate
                Console.WriteLine("Content of recordToUpdate:");
                Console.WriteLine(recordToUpdate != null ? JsonConvert.SerializeObject(recordToUpdate, Formatting.Indented) : "Record not found.");

                if (recordToUpdate != null)
                {
                    // Update the download status and used link
                    recordToUpdate.DownloadStatus = newDownloadStatus;
                    recordToUpdate.UsedLink = newUsedLink;

                    // Serialize the updated records back to JSON
                    string json = JsonConvert.SerializeObject(records, Formatting.Indented);

                    // Write the JSON back to the file
                    File.WriteAllText(downloadStatusReportPath, json);
                    Console.WriteLine($"Download status and used link for ID '{idToReplace}' replaced with '{newDownloadStatus}' and '{newUsedLink}'.");
                }
                else
                {
                    Console.WriteLine($"ID '{idToReplace}' not found in download status.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error replacing download status and used link: {ex.Message}");
            }
        }


    }
}