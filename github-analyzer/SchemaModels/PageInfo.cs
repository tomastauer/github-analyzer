namespace github_analyzer
{
    internal class PageInfo
    {
        public string EndCursor { get; set; }

        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public string StartCursor { get; set; }
    }
}