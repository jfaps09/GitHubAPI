using System.Collections.Generic;

namespace Domain
{
    public interface IMyReport
    {
        string path { get; set; }
        string fileName { get; set; }
        string fileExtension { get; set; }
        string title { get; set; }
        List<string[]> content { get; set; }
    }

    public class MyReport : IMyReport
    {
        public string path { get; set; }
        public string fileName { get; set; }
        public string fileExtension { get; set; }
        public string title { get; set; }
        public List<string[]> content { get; set; }

        public MyReport()
        {
        }
    }
}