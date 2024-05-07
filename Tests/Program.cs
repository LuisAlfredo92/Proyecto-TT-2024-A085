using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Tests.Identifying_data_tests.Name_tests;

namespace Tests;

public class Program
{
    // ReSharper disable once UnusedMember.Global
    [AttributeUsage(AttributeTargets.Class)]
    public class MyJobAttribute : Attribute, IConfigSource
    {
        protected internal MyJobAttribute()
        {
            Config = ManualConfig.CreateEmpty().AddJob(
                new Job("MyJob", RunMode.Dry, EnvironmentMode.RyuJitX64)
                {
                    Environment = { Runtime = CoreRuntime.Core80 },
                    Run = { LaunchCount = 10, IterationCount = 100},
                    Accuracy = { MaxRelativeError = 0.01 }
                });
        }

        public IConfig Config { get; }
    }

    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        /*if (args.Length == 0)
        {
            Console.WriteLine("Usage: Tests [data type]\nExample \"Tests names\"");
            return;
        }
        List<(Summary encryptSummary, Summary decryptSummary)> summaries = [];
        var data = args[0].ToLower();
        switch (data)
        {
            case "names":
                Summary encryption = BenchmarkRunner.Run<NameTests>(),
                    decryption = BenchmarkRunner.Run<DecryptNameTests>();
                summaries.Add((encryption, decryption));
                break;
        }*/
    }

}