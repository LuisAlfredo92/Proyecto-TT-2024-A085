using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public byte[] EncryptInsuranceIdentifierRsa() => _rsa.Encrypt(_insuranceIdentifier);

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedInsuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
        _insuranceIdentifier = _rsa.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public byte[] DecryptInsuranceIdentifierRsa() => _rsa.Decrypt(_insuranceIdentifier);
}