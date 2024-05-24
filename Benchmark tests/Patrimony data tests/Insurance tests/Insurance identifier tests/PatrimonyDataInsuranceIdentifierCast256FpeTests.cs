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
public class PatrimonyDataInsuranceIdentifierCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _insuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptInsuranceIdentifierCast256Fpe() => _cast256Fpe.Encrypt(_insuranceIdentifier);

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedInsuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _insuranceIdentifier = _cast256Fpe.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public char[] DecryptInsuranceIdentifierCast256Fpe() => _cast256Fpe.Decrypt(_insuranceIdentifier);
}