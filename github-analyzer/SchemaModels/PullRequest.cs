using System;

namespace github_analyzer
{
    internal class PullRequest
    {
        public bool Merged { get; set; }
        public DateTime? ClosedAt { get; set; }

        public Repository Repository { get; set; }
    }
}