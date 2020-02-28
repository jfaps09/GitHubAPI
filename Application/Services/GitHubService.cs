using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface IGitHubService
    {
        Task<IReadOnlyList<MyRepository>> GetRepos(string term);

        Task<IReadOnlyList<MyCommit>> GetRepoCommits(string owner, string name);

        Task<string> GetCommitDonwload(string owner, string name, string sha);
    }

    public class GitHubService : IGitHubService
    {
        private const string token = //Insert OAuth token here;
        private GitHubClient client;
        //private IConfigurationService configuration;

        public GitHubService()
        {
            GetGitHubClient("my-cool-app");
            //configuration = new ConfigurationService();
        }

        private void GetGitHubClient(string productHeaderValue)
        {
            this.client = new GitHubClient(new ProductHeaderValue(productHeaderValue));
            var tokenAuth = new Credentials(token);
            this.client.Credentials = tokenAuth;
        }

        public async Task<IReadOnlyList<MyRepository>> GetRepos(string term)
        {
            //ApiOptions opts = new ApiOptions();
            //opts.PageCount = 3;
            //opts.PageSize = 10;
            //await client.Repository.GetAllForUser(user, opts);

            var request = await client.Search.SearchRepo(new SearchRepositoriesRequest(term));
            var repositories = request.Items;

            List<MyRepository> repositoriesList = new List<MyRepository>();

            foreach (var r in repositories)
            {
                MyRepository rep = new MyRepository();
                rep.name = r.Name;
                rep.owner = r.Owner.Login;
                rep.mainLanguage = r.Language;
                rep.commits = new List<MyCommit>();
                rep.watchers = client.Activity.Watching.GetAllWatchers(r.Id).Result.Count;
                rep.stars = r.StargazersCount;
                rep.openIssues = r.OpenIssuesCount;
                rep.created = r.CreatedAt;
                rep.updated = r.UpdatedAt;
                rep.forks = r.ForksCount;
                rep.url = r.HtmlUrl;
                rep.allLanguages = new List<string>();
                var languages = await client.Repository.GetAllLanguages(r.Id);
                foreach (var l in languages)
                {
                    rep.allLanguages.Add(l.Name);
                }
                repositoriesList.Add(rep);
            }

            return repositoriesList;
        }

        public async Task<IReadOnlyList<MyCommit>> GetRepoCommits(string owner, string name)
        {
            var commits = await client.Repository.Commit.GetAll(owner, name);

            List<MyCommit> commitsList = new List<MyCommit>();

            foreach (var c in commits)
            {
                MyCommit newCom = new MyCommit();
                newCom.author = "unknown";
                if (c.Author != null)
                {
                    newCom.author = c.Author.Login;
                }
                newCom.comments = c.CommentsUrl;
                newCom.date = c.Commit.Committer.Date;
                newCom.url = c.HtmlUrl;
                newCom.sha = c.Sha;
                newCom.downloadUrl = c.HtmlUrl.Replace("commit/" + newCom.sha, "archive/" + newCom.sha) + ".zip";

                if (c.Stats != null)
                {
                    newCom.additions = c.Stats.Additions;
                    newCom.deletions = c.Stats.Deletions;
                }
                if (c.Files != null)
                {
                    newCom.files = new List<string>();
                    foreach (var f in c.Files)
                    {
                        newCom.files.Add(f.Filename);
                    }
                }
                commitsList.Add(newCom);
            }

            return commitsList;
        }

        public async Task<string> GetCommitDonwload(string owner, string name, string sha)
        {
            //Work in Progress

            var file = await client.Repository.Commit.Get(owner, name, sha);

            string downloadUrl = file.Commit.Tree.Url;

            return downloadUrl;
        }
    }
}
