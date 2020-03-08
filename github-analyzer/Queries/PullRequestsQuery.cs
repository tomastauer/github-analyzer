using GraphQL;

namespace github_analyzer
{

    internal class PullRequestsQuery : IQuery
    {
        private string Login { get; }
        private string Before { get; }

        public PullRequestsQuery(string login, string before)
        {
            Login = login;
            Before = before;
        }

        public GraphQLRequest CreateRequest()
        {
            return new GraphQLRequest
            {
                Query = @"
query User($login: String!, $before: String) {
    user(login: $login) {
        login
        pullRequests(last: 100, before: $before) {
            nodes {
                merged
                closedAt
                repository {
                    nameWithOwner
                }
            }
            pageInfo {
                hasPreviousPage
                startCursor
            }
        }
    }
}",
                OperationName = "User",
                Variables = new
                {
                    login = this.Login,
                    before = this.Before
                }
            };

        }
    }
}
