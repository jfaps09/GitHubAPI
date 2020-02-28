using System;
using System.Collections.Generic;

namespace Application
{
    public class MyCommit
    {
        public string author { get; set; }
        public string comments { get; set; }
        public DateTimeOffset date { get; set; }
        public string url { get; set; }
        public string downloadUrl { get; set; }
        public string sha { get; set; }
        public List<string> files { get; set; }
        public int additions { get; set; }
        public int deletions { get; set; }

        public MyCommit()
        {
        }
    }
}