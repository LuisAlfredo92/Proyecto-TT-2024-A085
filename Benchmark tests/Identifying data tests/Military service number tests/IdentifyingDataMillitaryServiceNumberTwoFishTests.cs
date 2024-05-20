using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _serviceNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptMillitaryServiceNumberTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _serviceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptMillitaryServiceNumberTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_serviceNumber);
    }

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedMillitaryServiceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
        _serviceNumber = _twoFish.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public byte[] DecryptMillitaryServiceNumberTwoFish() => _twoFish.Decrypt(_serviceNumber);
}