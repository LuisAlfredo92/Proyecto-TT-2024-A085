using System.Security.Cryptography;
using Academic_data.Referrals;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Referrals_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataReferralChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _referral = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptReferralChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _referral = File.ReadAllBytes(ReferralGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReferralChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_referral);
    }

    [GlobalSetup(Target = nameof(DecryptReferralChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedReferral = File.ReadAllBytes(ReferralGenerator.Generate());
        _referral = _chaCha20.Encrypt(generatedReferral);
    }

    [Benchmark]
    public byte[] DecryptReferralChaCha20() => _chaCha20.Decrypt(_referral);
}