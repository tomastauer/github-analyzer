using GraphQL.Client.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GraphQL;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task Run()
        {
            var pullRequests = (await GetAllClosedPullRequestsAfter(DateThreshold))
                .Where(p => p.Repository.NameWithOwner.StartsWith(RepositoryOwner))
                .GroupBy(p => p.Repository.NameWithOwner)
                .Select(g => new AnalyseResult { Repository = g.Key, Contributions = g.Count(), LastContribution = g.Max(c => c.ClosedAt.Value) })
                .OrderByDescending(r => r.Contributions)
                .ToList();

            pullRequests.ForEach(r =>
            {
                Console.WriteLine($"{r.Repository.PadRight(50, ' ')} \t\t {r.Contributions.ToString().PadRight(5, ' ')} \t\t {r.LastContribution}");
            });
        }

        private async Task<IEnumerable<PullRequest>> GetAllClosedPullRequestsAfter(DateTime threshold)
        {
            var pullRequests = new List<PullRequest>();
            string cursor = null;

            bool hasPreviousPage;
            do
            {
                var result = await Client.SendQueryAsync<UserResponse>(new PullRequestsQuery(Subject, cursor).CreateRequest());
                var connection = result.Data.User.PullRequests;

                pullRequests.InsertRange(0, connection.Nodes);

                cursor = connection.PageInfo.StartCursor;
                hasPreviousPage = connection.PageInfo.HasPreviousPage;
            } while (hasPreviousPage && pullRequests.First().ClosedAt > threshold);

            return pullRequests.Where(p => p.ClosedAt > threshold && p.Merged);
        }
    }
}
