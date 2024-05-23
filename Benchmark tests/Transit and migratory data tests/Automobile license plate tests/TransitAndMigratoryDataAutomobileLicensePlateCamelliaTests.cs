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
public class TransitAndMigratoryDataAutomobileLicensePlateCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _automobileLicensePlate = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptAutomobileLicensePlateCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAutomobileLicensePlateCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_automobileLicensePlate);
    }

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedAutomobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
        _automobileLicensePlate = _camellia.Encrypt(generatedAutomobileLicensePlate);
    }

    [Benchmark]
    public byte[] DecryptAutomobileLicensePlateCamellia() => _camellia.Decrypt(_automobileLicensePlate);
}