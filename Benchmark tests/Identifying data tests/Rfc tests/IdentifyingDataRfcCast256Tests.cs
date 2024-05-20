using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Rfc;

namespace Rfc_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataRfcCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _rfc = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptRfcCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptRfcCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_rfc);
    }

    [GlobalSetup(Target = nameof(DecryptRfcCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedRfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
        _rfc = _cast256.Encrypt(generatedRfc);
    }

    [Benchmark]
    public byte[] DecryptRfcCast256() => _cast256.Decrypt(_rfc);
}