using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Ideological_data.Civil_organizations;

namespace Civil_organizations_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataCivilOrganizationRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _civilOrganization = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptCivilOrganizationRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _civilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptCivilOrganizationRsa() => _rsa.Encrypt(_civilOrganization);

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedCivilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
        _civilOrganization = _rsa.Encrypt(generatedCivilOrganization);
    }

    [Benchmark]
    public byte[] DecryptCivilOrganizationRsa() => _rsa.Decrypt(_civilOrganization);
}