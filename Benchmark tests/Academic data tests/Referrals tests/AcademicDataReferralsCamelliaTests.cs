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
public class AcademicDataReferralCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _referral = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptReferralCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _referral = File.ReadAllBytes(ReferralGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReferralCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_referral);
    }

    [GlobalSetup(Target = nameof(DecryptReferralCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedReferral = File.ReadAllBytes(ReferralGenerator.Generate());
        _referral = _camellia.Encrypt(generatedReferral);
    }

    [Benchmark]
    public byte[] DecryptReferralCamellia() => _camellia.Decrypt(_referral);
}