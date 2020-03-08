namespace github_analyzer
{
    internal class Edge<T>
    {
        public string Cursor { get; set; }
        public T Node { get; set; }
    }
}