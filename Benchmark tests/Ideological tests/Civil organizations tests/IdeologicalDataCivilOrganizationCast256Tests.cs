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
public class IdeologicalDataCivilOrganizationCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _civilOrganization = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptCivilOrganizationCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _civilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCivilOrganizationCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_civilOrganization);
    }

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedCivilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
        _civilOrganization = _cast256.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public byte[] DecryptCivilOrganizationCast256() => _cast256.Decrypt(_civilOrganization);
}