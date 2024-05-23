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
public class TransitAndMigratoryDataAutomobileLicensePlateTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _automobileLicensePlate = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _automobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAutomobileLicensePlateTwoFishFpe() => _twoFishFpe.Encrypt(_automobileLicensePlate);

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedAutomobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate().ToCharArray();
        _automobileLicensePlate = _twoFishFpe.Encrypt(generatedAutomobileLicensePlate);
    }

    [Benchmark]
    public char[] DecryptAutomobileLicensePlateTwoFishFpe() => _twoFishFpe.Decrypt(_automobileLicensePlate);
}