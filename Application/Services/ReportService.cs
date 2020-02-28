using Domain;
using Infrastructure;
using System.Collections.Generic;

namespace Application.Services
{
    public interface IReportService
    {
        void CreateReport(List<MyRepository> content, string fileExtension);
    }

    public class ReportService : IReportService
    {
        private IOpenOffice openOffice;
        private IMyReport report;

        public ReportService()
        {
            openOffice = new OpenOffice();
            report = new MyReport();
        }

        public void CreateReport(List<MyRepository> content, string fileExtension)
        {
            var headerRow = new List<string[]>() {
                    new string[] {
                        "Name", "Owner", "Language", "Watchers", "Stars", "Open Issues",
                        "Forks", "Created at", "Updated at", "Link", "DL Link"
                    }
                };

            foreach (MyRepository r in content)
            {
                headerRow.Add(new string[] {
                        r.name, r.owner, r.mainLanguage, r.watchers.ToString(),
                        r.stars.ToString(), r.openIssues.ToString(), r.forks.ToString(),
                        r.created.ToString(), r.updated.ToString(), r.url, r.url + "/archive/master.zip"
                    }); ;
            }

            report.path = @"..\..\..\..\Files\";
            report.fileName = content[0].owner;
            report.fileExtension = fileExtension;
            report.title = "Repositories";
            report.content = headerRow;

            if (fileExtension.Equals("json"))
                openOffice.CreateJSONFile(report);
            else
                openOffice.CreateExcelFile(report);
        }
    }
}