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
public class IdeologicalDataCivilOrganizationCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _civilOrganization = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCivilOrganizationCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _civilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCivilOrganizationCast256Fpe() => _cast256Fpe.Encrypt(_civilOrganization);

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedCivilOrganization = CivilOrganizationsGenerator.Generate().ToCharArray();
        _civilOrganization = _cast256Fpe.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public char[] DecryptCivilOrganizationCast256Fpe() => _cast256Fpe.Decrypt(_civilOrganization);
}