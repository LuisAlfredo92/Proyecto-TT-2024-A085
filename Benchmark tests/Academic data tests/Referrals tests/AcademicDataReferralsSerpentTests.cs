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
public class AcademicDataReferralSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _referral = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptReferralSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _referral = File.ReadAllBytes(ReferralGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReferralSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_referral);
    }

    [GlobalSetup(Target = nameof(DecryptReferralSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedReferral = File.ReadAllBytes(ReferralGenerator.Generate());
        _referral = _serpent.Encrypt(generatedReferral);
    }

    [Benchmark]
    public byte[] DecryptReferralSerpent() => _serpent.Decrypt(_referral);
}