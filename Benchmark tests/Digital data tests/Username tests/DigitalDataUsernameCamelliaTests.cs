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
public class DigitalDataUsernameCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _username = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptUsernameCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _username = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUsernameCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_username);
    }

    [GlobalSetup(Target = nameof(DecryptUsernameCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedUsername = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
        _username = _camellia.Encrypt(generatedUsername);
    }

    [Benchmark]
    public byte[] DecryptUsernameCamellia() => _camellia.Decrypt(_username);
}