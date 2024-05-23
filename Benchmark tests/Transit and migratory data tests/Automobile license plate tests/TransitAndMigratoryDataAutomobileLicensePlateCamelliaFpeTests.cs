using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Automobile_license_plate;

namespace Automobile_license_plate_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataAutomobileLicensePlateCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _automobileLicensePlate = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _automobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAutomobileLicensePlateCamelliaFpe() => _camelliaFpe.Encrypt(_automobileLicensePlate);

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedAutomobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate().ToCharArray();
        _automobileLicensePlate = _camelliaFpe.Encrypt(generatedAutomobileLicensePlate);
    }

    [Benchmark]
    public char[] DecryptAutomobileLicensePlateCamelliaFpe() => _camelliaFpe.Decrypt(_automobileLicensePlate);
}