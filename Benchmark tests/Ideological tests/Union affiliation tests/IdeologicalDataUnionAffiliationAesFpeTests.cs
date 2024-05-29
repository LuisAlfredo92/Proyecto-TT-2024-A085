using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Ideological_data.Union_affiliation;

namespace Union_affiliation_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataUnionAffiliationAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _unionAffiliation = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUnionAffiliationAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _unionAffiliation = UnionAffiliationGenerator.GenerateId().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUnionAffiliationAesFpe() => _aesFpe.Encrypt(_unionAffiliation);

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedUnionAffiliation = UnionAffiliationGenerator.GenerateId().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
        _unionAffiliation = _aesFpe.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public char[] DecryptUnionAffiliationAesFpe() => _aesFpe.Decrypt(_unionAffiliation);
}