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
public class IdeologicalDataUnionAffiliationCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _unionAffiliation = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptUnionAffiliationCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _unionAffiliation = UnionAffiliationGenerator.GenerateId().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
    }

    [Benchmark]
    public char[] EncryptUnionAffiliationCamelliaFpe() => _camelliaFpe.Encrypt(_unionAffiliation);

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedUnionAffiliation = UnionAffiliationGenerator.GenerateId().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
        _unionAffiliation = _camelliaFpe.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public char[] DecryptUnionAffiliationCamelliaFpe() => _camelliaFpe.Decrypt(_unionAffiliation);
}