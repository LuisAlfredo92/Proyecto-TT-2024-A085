using BenchmarkDotNet.Running;

namespace Addresses_tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
