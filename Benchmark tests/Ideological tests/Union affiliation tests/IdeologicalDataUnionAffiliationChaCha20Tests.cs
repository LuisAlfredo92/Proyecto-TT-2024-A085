using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Ideological_data.Union_affiliation;
using Stream_ciphers;

namespace Union_affiliation_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataUnionAffiliationChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _unionAffiliation = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptUnionAffiliationChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _unionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUnionAffiliationChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_unionAffiliation);
    }

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedUnionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
        _unionAffiliation = _chaCha20.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public byte[] DecryptUnionAffiliationChaCha20() => _chaCha20.Decrypt(_unionAffiliation);
}