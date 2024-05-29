using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Ideological_data.Civil_organizations;

namespace Civil_organizations_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataCivilOrganizationCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _civilOrganization = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptCivilOrganizationCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _civilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCivilOrganizationCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_civilOrganization);
    }

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedCivilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
        _civilOrganization = _camellia.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public byte[] DecryptCivilOrganizationCamellia() => _camellia.Decrypt(_civilOrganization);
}