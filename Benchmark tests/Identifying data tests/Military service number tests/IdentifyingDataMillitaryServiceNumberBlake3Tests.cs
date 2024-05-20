using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Identifying_data.Curps;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberBlake3Tests
{
    private byte[] _serviceNumber = null!;

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberBlake3))]
    public void SetupEncryption()
    {
        _serviceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
    }

    [Benchmark]
    public Span<byte> EncryptMillitaryServiceNumberBlake3() => Blake3.Hash(_serviceNumber);
}