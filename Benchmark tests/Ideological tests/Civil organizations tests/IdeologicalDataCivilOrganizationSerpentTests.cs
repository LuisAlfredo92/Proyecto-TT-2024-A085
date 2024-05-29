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
public class IdeologicalDataCivilOrganizationSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _civilOrganization = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptCivilOrganizationSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _civilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCivilOrganizationSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_civilOrganization);
    }

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedCivilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
        _civilOrganization = _serpent.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public byte[] DecryptCivilOrganizationSerpent() => _serpent.Decrypt(_civilOrganization);
}