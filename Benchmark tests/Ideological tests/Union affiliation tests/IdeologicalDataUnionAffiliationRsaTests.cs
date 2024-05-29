using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Ideological_data.Union_affiliation;

namespace Union_affiliation_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataUnionAffiliationRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _unionAffiliation = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptUnionAffiliationRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _unionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
    }

    [Benchmark]
    public byte[] EncryptUnionAffiliationRsa() => _rsa.Encrypt(_unionAffiliation);

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedUnionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
        _unionAffiliation = _rsa.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public byte[] DecryptUnionAffiliationRsa() => _rsa.Decrypt(_unionAffiliation);
}