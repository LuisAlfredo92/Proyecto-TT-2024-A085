using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _insuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptInsuranceIdentifierSerpentFpe() => _serpentFpe.Encrypt(_insuranceIdentifier);

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedInsuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _insuranceIdentifier = _serpentFpe.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public char[] DecryptInsuranceIdentifierSerpentFpe() => _serpentFpe.Decrypt(_insuranceIdentifier);
}