using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Policies;
using Stream_ciphers;

namespace Policies_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataPoliciesChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _policies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptPoliciesChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _policies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPoliciesChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_policies);
    }

    [GlobalSetup(Target = nameof(DecryptPoliciesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedPolicies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
        _policies = _chaCha20.Encrypt(generatedPolicies);
    }

    [Benchmark]
    public byte[] DecryptPoliciesChaCha20() => _chaCha20.Decrypt(_policies);
}