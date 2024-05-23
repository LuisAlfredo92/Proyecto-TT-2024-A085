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
public class TransitAndMigratoryDataVisaCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _visa = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "CDJMTRNVL0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVisaCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _visa = VisaGenerator.GenerateVisaType().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVisaCast256Fpe() => _cast256Fpe.Encrypt(_visa);

    [GlobalSetup(Target = nameof(DecryptVisaCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedVisa = VisaGenerator.GenerateVisaType().ToCharArray();
        _visa = _cast256Fpe.Encrypt(generatedVisa);
    }

    [Benchmark]
    public char[] DecryptVisaCast256Fpe() => _cast256Fpe.Decrypt(_visa);
}