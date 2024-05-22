using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Digital_data.Usernames;
using FPE_ciphers;

namespace Username_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataUsernameTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _username = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUsernameTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _username = UsernamesGenerator.GenerateUsername().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUsernameTwoFishFpe() => _twoFishFpe.Encrypt(_username);

    [GlobalSetup(Target = nameof(DecryptUsernameTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedUsername = UsernamesGenerator.GenerateUsername().ToCharArray();
        _username = _twoFishFpe.Encrypt(generatedUsername);
    }

    [Benchmark]
    public char[] DecryptUsernameTwoFishFpe() => _twoFishFpe.Decrypt(_username);
}