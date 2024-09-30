using OfficeOpenXml;
using System.IO;

public static class ExcelService
{
    public static List<ExcelRow> GetTopProductsData(int id)
{
    var excelData = new List<ExcelRow>();
    var filePath = "top_3_products.xlsx"; // path might change

    using (var package = new ExcelPackage(new FileInfo(filePath)))
    {
        if (package.Workbook.Worksheets.Count == 0)
        {
            throw new Exception("No worksheets found in the Excel file.");
        }

        var worksheet = package.Workbook.Worksheets[0]; 
        var rowCount = worksheet.Dimension.Rows;

        for (int row = 2; row <= rowCount; row++) 
        {
            var idCellText = worksheet.Cells[row, 4].Text;

            // Check if the cell is empty or null
            if (string.IsNullOrWhiteSpace(idCellText))
            {
                continue; // Skip this row if the Id cell is empty
            }

            int rowId;
            if (int.TryParse(idCellText, out rowId))
            {
                string district = worksheet.Cells[row, 1].Text;
                string product = worksheet.Cells[row, 2].Text;
                double successPercentage = double.Parse(worksheet.Cells[row, 3].Text);

                if (rowId == id)
                {
                    excelData.Add(new ExcelRow { District = district, Product = product, SuccessPercentage = successPercentage });
                }

                if (excelData.Count == 3)
                    break;
            }
            else
            {
                throw new FormatException($"Invalid Id format in row {row}");
            }
        }
    }

    return excelData;
}

}



public class ExcelRow
{
    public string District { get; set; }
    public string Product { get; set; }
    public double SuccessPercentage { get; set; }
}
