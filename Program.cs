// See https://aka.ms/new-console-template for more information

using PdfGetter;

string filePathEditMe = "./EditMe.txt";
string filePathMyExcelFile;
string filePathExcelSheetName;

using (StreamReader reader = new StreamReader(filePathEditMe))
{
    filePathMyExcelFile = reader.ReadLine();
    filePathExcelSheetName = reader.ReadLine();

    if (filePathMyExcelFile == null || filePathExcelSheetName == null)
    {
        Console.Out.WriteLine("Missing file path or sheet name in EditMe.txt");
        return;
    }
}

var (ids, links1, links2) =
    MyExcelReader.ReadMyExcelFile(filePathMyExcelFile.Trim(), filePathExcelSheetName.Trim());

// ids = ids.Take(25).ToList();
// links1 = links1.Take(25).ToList();
// links2 = links2.Take(25).ToList();

await PdfToDownload.DownloadPdfOrHtmlAsync(ids, links1, links2);
DownloadedContentFiltering.RemoveSmallFiles("./downloadedContent");

// Wait for user input before exiting
Console.WriteLine("Press any key to exit...");
Console.ReadKey();