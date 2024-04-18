using OfficeOpenXml;

namespace PdfGetter;

public static class MyExcelReader
{
    public static (List<string?>, List<string?>, List<string?>) ReadMyExcelFile(string filePath, string worksheetName)
    {
        // if the file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine("The specified file does not exist!!");
            return (null, null, null)!;
        }

        List<string?> pdfListIds = [];
        List<string?> pdfListLinks1 = [];
        List<string?> pdfListLinks2 = [];

        // Excel package
        using ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
        ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetName]; // Sheet name

        if (worksheet != null)
        {
            // Number of rows in the worksheet
            int rowCount = worksheet.Dimension.End.Row;

            // Iterate over each row to extract values from columns A, AL, and AM
            for (int row = 2; row <= rowCount; row++)
            {
                string? pdfId = worksheet.Cells[row, 1].Value?.ToString(); // Column A
                string? pdfLink1 = worksheet.Cells[row, 38].Value?.ToString(); // Column AL
                string? pdfLink2 = worksheet.Cells[row, 39].Value?.ToString(); // Column AM

                pdfListIds.Add(pdfId);
                pdfListLinks1.Add(pdfLink1);
                pdfListLinks2.Add(pdfLink2);
            }
        }
        else
        {
            Console.WriteLine("The specified worksheet does not exist in the file!");
        }

        return (pdfListIds, pdfListLinks1, pdfListLinks2);
    }
}