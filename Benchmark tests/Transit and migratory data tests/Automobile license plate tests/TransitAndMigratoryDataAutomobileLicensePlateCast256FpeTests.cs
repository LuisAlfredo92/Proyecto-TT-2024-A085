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
public class TransitAndMigratoryDataAutomobileLicensePlateCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _automobileLicensePlate = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _automobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAutomobileLicensePlateCast256Fpe() => _cast256Fpe.Encrypt(_automobileLicensePlate);

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedAutomobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate().ToCharArray();
        _automobileLicensePlate = _cast256Fpe.Encrypt(generatedAutomobileLicensePlate);
    }

    [Benchmark]
    public char[] DecryptAutomobileLicensePlateCast256Fpe() => _cast256Fpe.Decrypt(_automobileLicensePlate);
}