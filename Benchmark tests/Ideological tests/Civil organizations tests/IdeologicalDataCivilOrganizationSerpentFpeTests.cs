using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Ideological_data.Civil_organizations;

namespace Civil_organizations_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataCivilOrganizationSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _civilOrganization = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCivilOrganizationSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _civilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCivilOrganizationSerpentFpe() => _serpentFpe.Encrypt(_civilOrganization);

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedCivilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
        _civilOrganization = _serpentFpe.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public char[] DecryptCivilOrganizationSerpentFpe() => _serpentFpe.Decrypt(_civilOrganization);
}