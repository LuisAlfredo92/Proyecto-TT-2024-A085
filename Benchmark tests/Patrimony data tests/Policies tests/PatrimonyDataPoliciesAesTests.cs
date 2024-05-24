using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Policies;
using Aes = BlockCiphers.Aes;

namespace Policies_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataPoliciesAesTests
{
    private Aes _aes = null!;
    private byte[] _policies = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptPoliciesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _policies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
    }

    [Benchmark]
    public byte[] EncryptPoliciesAes() => _aes.Encrypt(_policies, out _);

    [GlobalSetup(Target = nameof(DecryptPoliciesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedPolicies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
        _policies = _aes.Encrypt(generatedPolicies, out _tag);
    }

    [Benchmark]
    public byte[] DecryptPoliciesAes() => _aes.Decrypt(_policies, _tag);
}