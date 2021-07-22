using System;

namespace github_analyzer
{
    internal class PullRequest
    {
        public bool Merged { get; set; }
        public DateTime? ClosedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public Repository Repository { get; set; }

        public string Title { get; set; }

        public string HeadRefName { get; set; }

        public int Additions { get; set; }

        public int Deletions { get; set; }

        public Connection<Commit> Commits { get; set; }

        public User Author { get; set; }

        public int Number { get; set; }

        public string Id { get; set; }
    }
}