using Domain;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.IO;

namespace Infrastructure
{
    public interface IOpenOffice
    {
        void CreateExcelFile(IMyReport report);

        void CreateJSONFile(IMyReport report);
    }

    public class OpenOffice : IOpenOffice
    {
        public OpenOffice()
        {
        }

        public void CreateExcelFile(IMyReport report)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add(report.title);

                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets[report.title];

                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(report.content[0].Length + 64) + "1";

                worksheet.Cells[headerRange].Style.Font.Bold = true;
                worksheet.Cells[headerRange].Style.Font.Size = 14;
                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.DarkOliveGreen);
                worksheet.Cells[headerRange].Style.Locked = true;

                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(report.content);

                for (int c = 10; c <= 11; c++)
                {
                    for (int l = 2; l <= report.content.Count; l++)
                    {
                        string temp = worksheet.Cells[l, c].Text;
                        worksheet.Cells[l, c].Hyperlink = new ExcelHyperLink(worksheet.Cells[l, c].Text);
                        worksheet.Cells[l, c].Value = temp;
                    }
                }

                worksheet.Cells[2, 10, report.content.Count, 11].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                worksheet.Cells[2, 10, report.content.Count, 11].Style.Font.UnderLine = true;

                worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.WrapText = true;

                FileInfo excelFile = new FileInfo(report.path + report.fileName + "." + report.fileExtension);
                excel.SaveAs(excelFile);

                Console.WriteLine("Excel file created!");
            }
        }

        public void CreateJSONFile(IMyReport report)
        {
            report.content.RemoveAt(0);
            File.WriteAllText(report.path + report.fileName + "." + report.fileExtension, JsonConvert.SerializeObject(report.content));

            Console.WriteLine("JSON file created!");
        }
    }
}