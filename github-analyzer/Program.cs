using System;
using System.Threading.Tasks;
using CommandLine;

namespace github_analyzer
{

    class Program
    {
        static async Task Main(string[] args)
        {
            await new Analyzer("a", "Pipedrive", "36b9824ad699fb94f6e39560573e86643c545a70", "2021-01-01").Run();
            //await CommandLine.Parser.Default.ParseArguments<ClOptions>(args)
            //    .MapResult((ClOptions opts) => new Analyzer(opts.Subject, opts.RepositoryOwner, opts.ApiToken, opts.DateThreshold).Run(), errs => Task.FromResult(0));
        }
    }
}
