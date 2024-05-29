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
public class IdeologicalDataCivilOrganizationTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _civilOrganization = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCivilOrganizationTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _civilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCivilOrganizationTwoFishFpe() => _twoFishFpe.Encrypt(_civilOrganization);

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedCivilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
        _civilOrganization = _twoFishFpe.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public char[] DecryptCivilOrganizationTwoFishFpe() => _twoFishFpe.Decrypt(_civilOrganization);
}