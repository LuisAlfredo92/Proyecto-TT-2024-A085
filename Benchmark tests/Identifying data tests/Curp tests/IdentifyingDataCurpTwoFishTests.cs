using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Curps;

namespace Curp_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _curp = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptCurpTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCurpTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_curp);
    }

    [GlobalSetup(Target = nameof(DecryptNamesTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedCurp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
        _curp = _twoFish.Encrypt(generatedCurp);
    }

    [Benchmark]
    public byte[] DecryptNamesTwoFish() => _twoFish.Decrypt(_curp);
}