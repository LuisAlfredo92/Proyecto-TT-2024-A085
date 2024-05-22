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
public class DigitalDataUsernameCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _username = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUsernameCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _username = UsernamesGenerator.GenerateUsername().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUsernameCast256Fpe() => _cast256Fpe.Encrypt(_username);

    [GlobalSetup(Target = nameof(DecryptUsernameCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedUsername = UsernamesGenerator.GenerateUsername().ToCharArray();
        _username = _cast256Fpe.Encrypt(generatedUsername);
    }

    [Benchmark]
    public char[] DecryptUsernameCast256Fpe() => _cast256Fpe.Decrypt(_username);
}