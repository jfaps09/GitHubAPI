using Application.Services;
using System;
using System.Collections.Generic;

namespace Application
{
    internal class Program
    {
        private static string user, language, repoName;

        private static void Main(string[] args)
        {
            //var configuration = new ConfigurationService();
            IGitHubService service = new GitHubService();
            IReportService reportService = new ReportService();

            GetTerm();

            List<MyRepository> reposList = new List<MyRepository>(service.GetRepos("user:" + user + " language:" + language).Result);
            PrintReposNames(reposList);

            Console.WriteLine("Insert name of the repository from {0} to get its commits: ", user);
            repoName = Console.ReadLine();

            List<MyCommit> commitsList = new List<MyCommit>(service.GetRepoCommits(user, repoName).Result);
            Console.Clear();

            Console.WriteLine("Number of commits in " + repoName + ": " + commitsList.Count);

            Console.WriteLine("\n\n######   COMMIT STATS   ######\n");
            foreach (var c in commitsList)
            {
                Console.WriteLine("Author of commit {0}: {1}", c.sha, c.author);
                Console.WriteLine("URL: {0}", c.url);
                Console.WriteLine("Download URL: {0}", c.downloadUrl);
                Console.WriteLine("Date of Commit: {0}\n", c.date);
            }

            reportService.CreateReport(reposList, "xlsx");

            Console.ReadKey();
        }

        private static void GetTerm()
        {
            Console.WriteLine("Username: ");
            user = Console.ReadLine();

            Console.WriteLine("Language: ");
            language = Console.ReadLine();
        }

        private static void PrintReposNames(List<MyRepository> reposList)
        {
            Console.Clear();
            Console.WriteLine("######   Repositories from {0} written in {1}   ######\n", user, language);

            reposList.ForEach(r => Console.WriteLine("Name: {0}", r.name));
        }
    }
}