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
public class PatrimonyDataInsuranceIdentifierCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _insuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptInsuranceIdentifierCamelliaFpe() => _camelliaFpe.Encrypt(_insuranceIdentifier);

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedInsuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _insuranceIdentifier = _camelliaFpe.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public char[] DecryptInsuranceIdentifierCamelliaFpe() => _camelliaFpe.Decrypt(_insuranceIdentifier);
}