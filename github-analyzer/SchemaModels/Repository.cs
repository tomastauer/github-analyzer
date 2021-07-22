namespace github_analyzer
{
    internal class Repository
    {
        public string NameWithOwner { get; set; }

        public string Name { get; set; }

        public Connection<PullRequest> PullRequests { get; set; }
    }
}