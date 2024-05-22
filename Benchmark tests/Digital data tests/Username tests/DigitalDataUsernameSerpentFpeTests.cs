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
public class DigitalDataUsernameSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _username = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUsernameSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _username = UsernamesGenerator.GenerateUsername().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUsernameSerpentFpe() => _serpentFpe.Encrypt(_username);

    [GlobalSetup(Target = nameof(DecryptUsernameSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedUsername = UsernamesGenerator.GenerateUsername().ToCharArray();
        _username = _serpentFpe.Encrypt(generatedUsername);
    }

    [Benchmark]
    public char[] DecryptUsernameSerpentFpe() => _serpentFpe.Decrypt(_username);
}