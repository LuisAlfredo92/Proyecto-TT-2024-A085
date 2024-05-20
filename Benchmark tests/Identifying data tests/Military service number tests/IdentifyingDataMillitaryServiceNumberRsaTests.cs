using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _serviceNumber = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _serviceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
    }

    [Benchmark]
    public byte[] EncryptMillitaryServiceNumberRsa() => _rsa.Encrypt(_serviceNumber);

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedMillitaryServiceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
        _serviceNumber = _rsa.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public byte[] DecryptMillitaryServiceNumberRsa() => _rsa.Decrypt(_serviceNumber);
}