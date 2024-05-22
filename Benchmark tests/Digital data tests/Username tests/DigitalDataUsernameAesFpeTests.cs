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
public class DigitalDataUsernameAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _username = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUsernameAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _username = UsernamesGenerator.GenerateUsername().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUsernameAesFpe() => _aesFpe.Encrypt(_username);

    [GlobalSetup(Target = nameof(DecryptUsernameAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedUsername = UsernamesGenerator.GenerateUsername().ToCharArray();
        _username = _aesFpe.Encrypt(generatedUsername);
    }

    [Benchmark]
    public char[] DecryptUsernameAesFpe() => _aesFpe.Decrypt(_username);
}