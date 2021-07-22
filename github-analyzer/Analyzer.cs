using GraphQL.Client.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GraphQL;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace github_analyzer
{
    class AnalyseResult
    {
        public string Repository { get; set; }

        public int Contributions { get; set; }

        public DateTime LastContribution { get; set; }
    }

    internal class Analyzer
    {
        private string Subject { get; }
        public string RepositoryOwner { get; }
        private GraphQLHttpClient Client { get; }
        public string ApiToken { get; }
        public DateTime DateThreshold { get; }

        public Analyzer(string subject, string repositoryOwner, string apiToken, string dateThreshold)
        {
            Subject = subject;
            RepositoryOwner = repositoryOwner;
            ApiToken = apiToken;
            DateThreshold = DateTime.Parse(dateThreshold);

            Client = SetupClient();
        }

        private GraphQLHttpClient SetupClient()
        {
            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://api.github.com/graphql"),
                JsonSerializer = new GraphQL.Client.Serializer.Newtonsoft.NewtonsoftJsonSerializer()
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", ApiToken);

            client.DefaultRequestHeaders.Add("User-agent", "github-analyzer");

            return client.AsGraphQLClient(options);
        }

        //public async Task Run()
        //{
        //    var pullRequests = (await GetAllClosedPullRequestsAfter(DateThreshold))
        //        .Where(p => p.Repository.NameWithOwner.StartsWith(RepositoryOwner))
        //        .GroupBy(p => p.Repository.NameWithOwner)
        //        .Select(g => new AnalyseResult { Repository = g.Key, Contributions = g.Count(), LastContribution = g.Max(c => c.ClosedAt.Value) })
        //        .OrderByDescending(r => r.Contributions)
        //        .ToList();

        //    pullRequests.ForEach(r =>
        //    {
        //        Console.WriteLine($"{r.Repository.PadRight(50, ' ')} \t\t {r.Contributions.ToString().PadRight(5, ' ')} \t\t {r.LastContribution}");
        //    });
        //}

        public async Task Run()
        {

            var touchedRepos = new List<string> {
                "kafka-search-queue-consumer",
                "recent-items",
                "entity-automation-proxy",
                "leadbox-service",
                "identity",
                "Pipedrive",
                "webapp",
                "leadbooster-chat",
                "webhooks-dealer",
                "backoffice-api",
                "api-docs",
                "web-forms-service",
                "leads-to-filters-sync",
                "tanker",
                "lead-custom-field-definition-consistency-check",
                "add-modals",
                "leadbox-fe",
                "leadfeeder-service",
                "db-version",
                "debezium",
                "search",
                "elastic-migrator",
                "domain-events-analyzer",
                "convert-to-lead",
                "packages",
                "leads-graphql",
                "backoffice-ui",
                "import-service",
                "prospector-service",
                "import-service-frontend",
                "import-service-core"
            };

            var pullRequests = new List<PullRequest>();

            foreach(var repo in touchedRepos)
            {
                pullRequests.AddRange(await GetAllLCDPullRequests(repo));
            }

            var serialized = JsonSerializer.Serialize(pullRequests);

            File.WriteAllText("C:/Downloads/output.json", serialized);


            var s = new StringBuilder("created at;name;branch;number;title;commits;additions;deletions;author");
            pullRequests.ForEach(p =>
            {
                var l = new List<string>() {
                    p.CreatedAt.ToString("o"),
                    p.Repository.NameWithOwner,
                    p.HeadRefName,
                    p.Number.ToString(),
                    p.Title,
                    p.Commits.TotalCount.ToString(),
                    p.Additions.ToString(),
                    p.Deletions.ToString(),
                    p.Author.Login,
                };

                s.AppendLine(String.Join(";", l));
            });

            File.WriteAllText("C:/Downloads/output.csv", s.ToString());

            pullRequests.ForEach(r =>
            {
                Console.WriteLine($"{r.Number.ToString().PadRight(5, ' ')}\t{r.Author.Login.PadRight(15, ' ')}\t{r.Title.ToString().PadRight(50, ' ')}\t{r.CreatedAt}");
            });
        }


        private async Task<IEnumerable<PullRequest>> GetAllClosedPullRequestsAfter(DateTime threshold)
        {
            var pullRequests = new List<PullRequest>();
            string cursor = null;

            bool hasPreviousPage;
            do
            {
                var result = await Client.SendQueryAsync<UserResponse>(new PullRequestsQuery(Subject, cursor).GetAllUsersPullRequests());
                var connection = result.Data.User.PullRequests;

                pullRequests.InsertRange(0, connection.Nodes);

                cursor = connection.PageInfo.StartCursor;
                hasPreviousPage = connection.PageInfo.HasPreviousPage;
            } while (hasPreviousPage && pullRequests.First().ClosedAt > threshold);

            return pullRequests.Where(p => p.ClosedAt > threshold && p.Merged);
        }

        private async Task<IEnumerable<PullRequest>> GetAllLCDPullRequests(string repo)
        {
            var pullRequests = new List<PullRequest>();
            string cursor = null;

            bool hasPreviousPage;
            do
            {
                var result = await Client.SendQueryAsync<RepositoryResponse>(new PullRequestsQuery(Subject, cursor).GetLast100PullRequestsFromRepositoryBefore(this.RepositoryOwner, repo, cursor));
                var connection = result.Data.Repository.PullRequests;

                pullRequests.InsertRange(0, connection.Nodes);


                Console.WriteLine($"Analyzing repository '{repo}', pull requests created after '{pullRequests.First().CreatedAt.ToLongDateString()}'");

                cursor = connection.PageInfo.StartCursor;
                hasPreviousPage = connection.PageInfo.HasPreviousPage;
            } while (hasPreviousPage && pullRequests.First().CreatedAt > this.DateThreshold);

            return pullRequests.Where(p => p.CreatedAt > this.DateThreshold && (p.Title.StartsWith("LCD", StringComparison.InvariantCultureIgnoreCase) || p.HeadRefName.StartsWith("LCD", StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
