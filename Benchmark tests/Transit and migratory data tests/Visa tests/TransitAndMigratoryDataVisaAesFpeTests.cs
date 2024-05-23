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
public class TransitAndMigratoryDataVisaAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _visa = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "CDJMTRNVL0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVisaAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _visa = VisaGenerator.GenerateVisaType().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVisaAesFpe() => _aesFpe.Encrypt(_visa);

    [GlobalSetup(Target = nameof(DecryptVisaAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedVisa = VisaGenerator.GenerateVisaType().ToCharArray();
        _visa = _aesFpe.Encrypt(generatedVisa);
    }

    [Benchmark]
    public char[] DecryptVisaAesFpe() => _aesFpe.Decrypt(_visa);
}