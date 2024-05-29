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
public class IdeologicalDataUnionAffiliationCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _unionAffiliation = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptUnionAffiliationCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _unionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUnionAffiliationCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_unionAffiliation);
    }

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedUnionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
        _unionAffiliation = _cast256.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public byte[] DecryptUnionAffiliationCast256() => _cast256.Decrypt(_unionAffiliation);
}