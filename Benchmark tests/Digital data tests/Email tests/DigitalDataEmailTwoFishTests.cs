using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Digital_data.Email;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataEmailTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _email = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptEmailTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _email = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEmailTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_email);
    }

    [GlobalSetup(Target = nameof(DecryptEmailTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedEmail = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
        _email = _twoFish.Encrypt(generatedEmail);
    }

    [Benchmark]
    public byte[] DecryptEmailTwoFish() => _twoFish.Decrypt(_email);
}