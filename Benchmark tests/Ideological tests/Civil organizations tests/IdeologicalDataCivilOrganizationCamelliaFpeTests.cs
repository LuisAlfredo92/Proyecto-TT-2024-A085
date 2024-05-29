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
public class IdeologicalDataCivilOrganizationCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _civilOrganization = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCivilOrganizationCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _civilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCivilOrganizationCamelliaFpe() => _camelliaFpe.Encrypt(_civilOrganization);

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedCivilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
        _civilOrganization = _camelliaFpe.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public char[] DecryptCivilOrganizationCamelliaFpe() => _camelliaFpe.Decrypt(_civilOrganization);
}