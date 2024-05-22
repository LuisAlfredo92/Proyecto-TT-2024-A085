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
public class DigitalDataUsernameCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _username = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUsernameCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _username = UsernamesGenerator.GenerateUsername().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUsernameCamelliaFpe() => _camelliaFpe.Encrypt(_username);

    [GlobalSetup(Target = nameof(DecryptUsernameCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedUsername = UsernamesGenerator.GenerateUsername().ToCharArray();
        _username = _camelliaFpe.Encrypt(generatedUsername);
    }

    [Benchmark]
    public char[] DecryptUsernameCamelliaFpe() => _camelliaFpe.Decrypt(_username);
}