namespace github_analyzer
{

    internal class User
    {
        public string Login { get; set; }

        public Connection<PullRequest> PullRequests { get; set; }
    }
}