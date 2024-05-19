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
public class IdentifyingDataRfcSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _rfc = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptRfcSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptRfcSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_rfc);
    }

    [GlobalSetup(Target = nameof(DecryptRfcSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedRfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
        _rfc = _serpent.Encrypt(generatedRfc);
    }

    [Benchmark]
    public byte[] DecryptRfcSerpent() => _serpent.Decrypt(_rfc);
}