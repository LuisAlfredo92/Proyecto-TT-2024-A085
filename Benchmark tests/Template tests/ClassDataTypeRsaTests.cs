using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class ClassDataTypeRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _yourData = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptTypeRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _yourData = BitConverter.GetBytes(TypeGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public byte[] EncryptTypeRsa() => _rsa.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedDate = BitConverter.GetBytes(TypeGenerator.GenerateBornDate().Ticks);
        _yourData = _rsa.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptTypeRsa() => _rsa.Decrypt(_yourData);
}