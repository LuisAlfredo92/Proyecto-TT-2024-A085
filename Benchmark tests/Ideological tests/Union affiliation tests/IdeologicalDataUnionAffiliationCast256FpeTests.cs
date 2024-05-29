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
public class IdeologicalDataUnionAffiliationCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _unionAffiliation = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUnionAffiliationCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _unionAffiliation = UnionAffiliationGenerator.GenerateId().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUnionAffiliationCast256Fpe() => _cast256Fpe.Encrypt(_unionAffiliation);

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedUnionAffiliation = UnionAffiliationGenerator.GenerateId().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
        _unionAffiliation = _cast256Fpe.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public char[] DecryptUnionAffiliationCast256Fpe() => _cast256Fpe.Decrypt(_unionAffiliation);
}