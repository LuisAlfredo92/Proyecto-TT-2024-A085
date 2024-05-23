using BenchmarkDotNet.Running;

namespace Automobile_license_plate_tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
