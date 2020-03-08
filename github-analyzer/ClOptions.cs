using CommandLine;

namespace github_analyzer
{
    internal class ClOptions
    {
        [Option('s', Required = true, HelpText = "Github login of the subject")]
        public string Subject { get; set; }

        [Option('r', Required = true, HelpText = "Github repository owner (analytics will be done only against the owner's repos")]
        public string RepositoryOwner { get; set; }

        [Option('t', Required = true, HelpText = "Github API token")]
        public string ApiToken{ get; set; }

        [Option('d', Required = true, HelpText = "Date threshold, PRs older than this will be ignored. Use format yyyy-mm-dd")]
        public string DateThreshold { get; set; }
    }
}
