using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Digital_data.Email;
using Aes = BlockCiphers.Aes;

namespace Email_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class DigitalDataEmailAesTests
{
    private Aes _aes = null!;
    private byte[] _email = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptEmailAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _email = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
    }

    [Benchmark]
    public byte[] EncryptEmailAes() => _aes.Encrypt(_email, out _);

    [GlobalSetup(Target = nameof(DecryptEmailAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedEmail = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
        _email = _aes.Encrypt(generatedEmail, out _tag);
    }

    [Benchmark]
    public byte[] DecryptEmailAes() => _aes.Decrypt(_email, _tag);
}