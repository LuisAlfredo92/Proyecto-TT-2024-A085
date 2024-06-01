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
public class PatrimonyDataPoliciesSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _policies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptPoliciesSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _policies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPoliciesSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_policies);
    }

    [GlobalSetup(Target = nameof(DecryptPoliciesSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedPolicies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
        _policies = _serpent.Encrypt(generatedPolicies);
    }

    [Benchmark]
    public byte[] DecryptPoliciesSerpent() => _serpent.Decrypt(_policies);
}