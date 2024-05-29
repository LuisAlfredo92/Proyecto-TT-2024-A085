using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Ideological_data.Civil_organizations;
using Aes = BlockCiphers.Aes;

namespace Civil_organizations_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataCivilOrganizationAesTests
{
    private Aes _aes = null!;
    private byte[] _civilOrganization = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptCivilOrganizationAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _civilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptCivilOrganizationAes() => _aes.Encrypt(_civilOrganization, out _);

    [GlobalSetup(Target = nameof(DecryptCivilOrganizationAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedCivilOrganization = Encoding.UTF8.GetBytes(CivilOrganizationsGenerator.Generate());
        _civilOrganization = _aes.Encrypt(generatedCivilOrganization, out _tag);
    }

    [Benchmark]
    public byte[] DecryptCivilOrganizationAes() => _aes.Decrypt(_civilOrganization, _tag);
}