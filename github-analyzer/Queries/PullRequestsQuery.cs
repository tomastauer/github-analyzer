using GraphQL;
using System;

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

        public GraphQLRequest GetAllUsersPullRequests()
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

        public GraphQLRequest GetLast100PullRequestsFromRepositoryBefore(string owner, string repo, string before)
        {
            return new GraphQLRequest
            {
                Query = @"
query($owner: String!, $repo: String!, $before: String) {
    repository(owner: $owner, name: $repo) {
        pullRequests(last: 100, before: $before) {
            pageInfo {
                startCursor
                hasPreviousPage
            }
            nodes {
                id
                number
                createdAt
                title
                author {
                    ...on User {
                        name
                        id
                        login
                    }
                }
                repository {
                    name
                    nameWithOwner
                }
                headRefName
                additions
                deletions
                commits {
                        totalCount
                }
            }
        }
    }
}
",
                Variables = new
                {
                    owner,
                    repo,
                    before
                }

            };
        }
    }
}
