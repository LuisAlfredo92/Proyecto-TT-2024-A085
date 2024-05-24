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
public class PatrimonyDataInsuranceIdentifierTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _insuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptInsuranceIdentifierTwoFishFpe() => _twoFishFpe.Encrypt(_insuranceIdentifier);

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedInsuranceIdentifier = Random.Shared.NextInt64().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _insuranceIdentifier = _twoFishFpe.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public char[] DecryptInsuranceIdentifierTwoFishFpe() => _twoFishFpe.Decrypt(_insuranceIdentifier);
}