using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Digital_data.Usernames;

namespace Username_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataUsernameTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _username = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptUsernameTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _username = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUsernameTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_username);
    }

    [GlobalSetup(Target = nameof(DecryptUsernameTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedUsername = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
        _username = _twoFish.Encrypt(generatedUsername);
    }

    [Benchmark]
    public byte[] DecryptUsernameTwoFish() => _twoFish.Decrypt(_username);
}