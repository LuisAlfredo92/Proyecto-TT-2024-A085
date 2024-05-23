using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Visa;

namespace Visa_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVisaTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _visa = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "CDJMTRNVL0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVisaTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _visa = VisaGenerator.GenerateVisaType().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVisaTwoFishFpe() => _twoFishFpe.Encrypt(_visa);

    [GlobalSetup(Target = nameof(DecryptVisaTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedVisa = VisaGenerator.GenerateVisaType().ToCharArray();
        _visa = _twoFishFpe.Encrypt(generatedVisa);
    }

    [Benchmark]
    public char[] DecryptVisaTwoFishFpe() => _twoFishFpe.Decrypt(_visa);
}