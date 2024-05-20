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
public class IdentifyingDataMillitaryServiceNumberPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _serviceNumber = null!;

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberPbkdf2))]
    public void SetupEncryption()
    {
        _serviceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
    }

    [Benchmark]
    public Span<byte> EncryptMillitaryServiceNumberPbkdf2() => _argon2Id.Hash(_serviceNumber);
}