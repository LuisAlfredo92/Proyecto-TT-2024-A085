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
public class TransitAndMigratoryDataVisaCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _visa = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "CDJMTRNVL0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVisaCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _visa = VisaGenerator.GenerateVisaType().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVisaCamelliaFpe() => _camelliaFpe.Encrypt(_visa);

    [GlobalSetup(Target = nameof(DecryptVisaCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedVisa = VisaGenerator.GenerateVisaType().ToCharArray();
        _visa = _camelliaFpe.Encrypt(generatedVisa);
    }

    [Benchmark]
    public char[] DecryptVisaCamelliaFpe() => _camelliaFpe.Decrypt(_visa);
}