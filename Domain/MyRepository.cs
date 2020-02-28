using System;
using System.Collections.Generic;

namespace Application
{
    public class MyRepository
    {
        public string name { get; set; }
        public string owner { get; set; }
        public string mainLanguage { get; set; }
        public List<MyCommit> commits { get; set; }
        public int watchers { get; set; }
        public int stars { get; set; }
        public int openIssues { get; set; }
        public int forks { get; set; }
        public DateTimeOffset created { get; set; }
        public DateTimeOffset updated { get; set; }
        public string url { get; set; }
        public List<string> allLanguages { get; set; }

        public MyRepository()
        {
        }
    }
}