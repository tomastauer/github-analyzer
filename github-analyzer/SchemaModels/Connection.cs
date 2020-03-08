#nullable enable

using System.Collections.Generic;

namespace github_analyzer
{
    internal class Connection<T>
    {
        public string? Edges { get; set; }

        public T[]? Nodes { get; set; }

        public PageInfo? PageInfo { get; set; }

        public int? TotalCount { get; set; }
    }
}