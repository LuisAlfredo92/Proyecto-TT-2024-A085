using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BlockCiphers;

namespace Tests;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Tests>();
    }

}

[MemoryDiagnoser]
//[ShortRunJob]
[VeryLongRunJob]
public class Tests
{
    private BlockCiphers.Aes _aes;

    [GlobalSetup]
    public void Setup()
    {
        //_aes = new Aes();
    }

    [Benchmark]
    public void AesEncryption()
    {

    }
}