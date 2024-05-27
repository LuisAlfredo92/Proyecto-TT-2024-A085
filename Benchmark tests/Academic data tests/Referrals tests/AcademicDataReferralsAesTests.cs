using System.Security.Cryptography;
using Academic_data.Referrals;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Referrals_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataReferralAesTests
{
    private Aes _aes = null!;
    private byte[] _referral = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptReferralAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _referral = File.ReadAllBytes(ReferralGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptReferralAes() => _aes.Encrypt(_referral, out _);

    [GlobalSetup(Target = nameof(DecryptReferralAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedReferral = File.ReadAllBytes(ReferralGenerator.Generate());
        _referral = _aes.Encrypt(generatedReferral, out _tag);
    }

    [Benchmark]
    public byte[] DecryptReferralAes() => _aes.Decrypt(_referral, _tag);
}