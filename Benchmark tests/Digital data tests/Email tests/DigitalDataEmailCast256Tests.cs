using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Digital_data.Email;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class DigitalDataEmailCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _email = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptEmailCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _email = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEmailCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_email);
    }

    [GlobalSetup(Target = nameof(DecryptEmailCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedEmail = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
        _email = _cast256.Encrypt(generatedEmail);
    }

    [Benchmark]
    public byte[] DecryptEmailCast256() => _cast256.Decrypt(_email);
}