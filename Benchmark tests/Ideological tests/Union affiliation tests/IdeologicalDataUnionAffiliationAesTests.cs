using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Ideological_data.Union_affiliation;
using Aes = BlockCiphers.Aes;

namespace Union_affiliation_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataUnionAffiliationAesTests
{
    private Aes _aes = null!;
    private byte[] _unionAffiliation = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptUnionAffiliationAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _unionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
    }

    [Benchmark]
    public byte[] EncryptUnionAffiliationAes() => _aes.Encrypt(_unionAffiliation, out _);

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedUnionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
        _unionAffiliation = _aes.Encrypt(generatedUnionAffiliation, out _tag);
    }

    [Benchmark]
    public byte[] DecryptUnionAffiliationAes() => _aes.Decrypt(_unionAffiliation, _tag);
}