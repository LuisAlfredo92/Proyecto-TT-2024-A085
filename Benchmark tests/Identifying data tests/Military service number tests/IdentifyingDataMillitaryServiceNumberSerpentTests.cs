using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _serviceNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptMillitaryServiceNumberSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _serviceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptMillitaryServiceNumberSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_serviceNumber);
    }

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedMillitaryServiceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
        _serviceNumber = _serpent.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public byte[] DecryptMillitaryServiceNumberSerpent() => _serpent.Decrypt(_serviceNumber);
}