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
public class TransitAndMigratoryDataVisaSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _visa = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "CDJMTRNVL0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVisaSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _visa = VisaGenerator.GenerateVisaType().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVisaSerpentFpe() => _serpentFpe.Encrypt(_visa);

    [GlobalSetup(Target = nameof(DecryptVisaSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedVisa = VisaGenerator.GenerateVisaType().ToCharArray();
        _visa = _serpentFpe.Encrypt(generatedVisa);
    }

    [Benchmark]
    public char[] DecryptVisaSerpentFpe() => _serpentFpe.Decrypt(_visa);
}