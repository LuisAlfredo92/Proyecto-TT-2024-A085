using System.Security.Cryptography;
using Academic_data.Referrals;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Referrals_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataReferralCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _referral = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptReferralCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _referral = File.ReadAllBytes(ReferralGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReferralCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_referral);
    }

    [GlobalSetup(Target = nameof(DecryptReferralCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedReferral = File.ReadAllBytes(ReferralGenerator.Generate());
        _referral = _cast256.Encrypt(generatedReferral);
    }

    [Benchmark]
    public byte[] DecryptReferralCast256() => _cast256.Decrypt(_referral);
}