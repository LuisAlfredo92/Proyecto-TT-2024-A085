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
public class IdentifyingDataRfcTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _rfc = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptRfcTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptRfcTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_rfc);
    }

    [GlobalSetup(Target = nameof(DecryptRfcTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedRfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
        _rfc = _twoFish.Encrypt(generatedRfc);
    }

    [Benchmark]
    public byte[] DecryptRfcTwoFish() => _twoFish.Decrypt(_rfc);
}