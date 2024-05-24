using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Policies;

namespace Policies_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataPoliciesCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _policies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptPoliciesCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _policies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPoliciesCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_policies);
    }

    [GlobalSetup(Target = nameof(DecryptPoliciesCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedPolicies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
        _policies = _cast256.Encrypt(generatedPolicies);
    }

    [Benchmark]
    public byte[] DecryptPoliciesCast256() => _cast256.Decrypt(_policies);
}