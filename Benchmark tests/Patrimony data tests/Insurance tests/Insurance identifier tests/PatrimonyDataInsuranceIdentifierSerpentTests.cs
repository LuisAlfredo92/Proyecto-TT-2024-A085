using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptInsuranceIdentifierSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceIdentifierSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_insuranceIdentifier);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedInsuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
        _insuranceIdentifier = _serpent.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public byte[] DecryptInsuranceIdentifierSerpent() => _serpent.Decrypt(_insuranceIdentifier);
}