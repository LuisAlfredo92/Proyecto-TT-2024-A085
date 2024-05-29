using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Ideological_data.Union_affiliation;

namespace Union_affiliation_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataUnionAffiliationSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _unionAffiliation = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptUnionAffiliationSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _unionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUnionAffiliationSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_unionAffiliation);
    }

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedUnionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
        _unionAffiliation = _serpent.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public byte[] DecryptUnionAffiliationSerpent() => _serpent.Decrypt(_unionAffiliation);
}