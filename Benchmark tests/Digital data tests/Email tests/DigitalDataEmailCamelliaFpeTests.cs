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
public class DigitalDataEmailCamelliaFpeTests
{
    private CamelliaFpe _serpentFpe = null!;
    private char[] _email = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@_.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEmailCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _email = EmailGenerator.GenerateEmail().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEmailCamelliaFpe() => _serpentFpe.Encrypt(_email);

    [GlobalSetup(Target = nameof(DecryptEmailCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedEmail = EmailGenerator.GenerateEmail().ToCharArray();
        _email = _serpentFpe.Encrypt(generatedEmail);
    }

    [Benchmark]
    public char[] DecryptEmailCamelliaFpe() => _serpentFpe.Decrypt(_email);
}