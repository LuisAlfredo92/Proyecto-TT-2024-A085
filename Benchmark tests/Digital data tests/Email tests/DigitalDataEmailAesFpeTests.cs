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
public class DigitalDataEmailAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _email = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@_.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEmailAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _email = EmailGenerator.GenerateEmail().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEmailAesFpe() => _aesFpe.Encrypt(_email);

    [GlobalSetup(Target = nameof(DecryptEmailAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedEmail = EmailGenerator.GenerateEmail().ToCharArray();
        _email = _aesFpe.Encrypt(generatedEmail);
    }

    [Benchmark]
    public char[] DecryptEmailAesFpe() => _aesFpe.Decrypt(_email);
}