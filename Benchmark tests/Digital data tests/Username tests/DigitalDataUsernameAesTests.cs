using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Digital_data.Usernames;
using Aes = BlockCiphers.Aes;

namespace Username_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataUsernameAesTests
{
    private Aes _aes = null!;
    private byte[] _username = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptUsernameAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _username = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
    }

    [Benchmark]
    public byte[] EncryptUsernameAes() => _aes.Encrypt(_username, out _);

    [GlobalSetup(Target = nameof(DecryptUsernameAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedUsername = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
        _username = _aes.Encrypt(generatedUsername, out _tag);
    }

    [Benchmark]
    public byte[] DecryptUsernameAes() => _aes.Decrypt(_username, _tag);
}