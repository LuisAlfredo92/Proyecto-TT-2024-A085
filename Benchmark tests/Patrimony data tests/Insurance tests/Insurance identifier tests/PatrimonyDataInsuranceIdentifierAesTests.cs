using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierAesTests
{
    private Aes _aes = null!;
    private byte[] _insuranceIdentifier = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public byte[] EncryptInsuranceIdentifierAes() => _aes.Encrypt(_insuranceIdentifier, out _);

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedInsuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
        _insuranceIdentifier = _aes.Encrypt(generatedInsuranceIdentifier, out _tag);
    }

    [Benchmark]
    public byte[] DecryptInsuranceIdentifierAes() => _aes.Decrypt(_insuranceIdentifier, _tag);
}