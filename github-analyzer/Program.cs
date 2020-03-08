using System;
using System.Threading.Tasks;
using CommandLine;

namespace github_analyzer
{

    class Program
    {
        static async Task Main(string[] args)
        {
            await CommandLine.Parser.Default.ParseArguments<ClOptions>(args)
                .MapResult((ClOptions opts) => new Analyzer(opts.Subject, opts.RepositoryOwner, opts.ApiToken, opts.DateThreshold).Run(), errs => Task.FromResult(0));
        }
    }
}
