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
public class TransitAndMigratoryDataAutomobileLicensePlateSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _automobileLicensePlate = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptAutomobileLicensePlateSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAutomobileLicensePlateSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_automobileLicensePlate);
    }

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedAutomobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
        _automobileLicensePlate = _serpent.Encrypt(generatedAutomobileLicensePlate);
    }

    [Benchmark]
    public byte[] DecryptAutomobileLicensePlateSerpent() => _serpent.Decrypt(_automobileLicensePlate);
}