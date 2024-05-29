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
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class IdeologicalDataCivilOrganizationTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _civilOrganization = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptCivilOrganizationTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _civilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCivilOrganizationTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_civilOrganization);
    }

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedCivilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
        _civilOrganization = _twoFish.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public byte[] DecryptCivilOrganizationTwoFish() => _twoFish.Decrypt(_civilOrganization);
}