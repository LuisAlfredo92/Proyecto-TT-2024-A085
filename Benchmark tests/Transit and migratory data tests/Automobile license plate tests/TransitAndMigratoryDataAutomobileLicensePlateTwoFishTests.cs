using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Transit_and_migratory_data.Automobile_license_plate;

namespace Automobile_license_plate_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataAutomobileLicensePlateTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _automobileLicensePlate = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptAutomobileLicensePlateTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAutomobileLicensePlateTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_automobileLicensePlate);
    }

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedAutomobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
        _automobileLicensePlate = _twoFish.Encrypt(generatedAutomobileLicensePlate);
    }

    [Benchmark]
    public byte[] DecryptAutomobileLicensePlateTwoFish() => _twoFish.Decrypt(_automobileLicensePlate);
}