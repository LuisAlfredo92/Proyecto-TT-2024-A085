using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Digital_data.Email;
using FPE_ciphers;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataEmailTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _email = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@_.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEmailTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _email = EmailGenerator.GenerateEmail().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEmailTwoFishFpe() => _twoFishFpe.Encrypt(_email);

    [GlobalSetup(Target = nameof(DecryptEmailTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedEmail = EmailGenerator.GenerateEmail().ToCharArray();
        _email = _twoFishFpe.Encrypt(generatedEmail);
    }

    [Benchmark]
    public char[] DecryptEmailTwoFishFpe() => _twoFishFpe.Decrypt(_email);
}