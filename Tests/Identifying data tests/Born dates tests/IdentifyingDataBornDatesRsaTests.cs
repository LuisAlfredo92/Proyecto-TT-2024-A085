using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Born_dates;

namespace Tests.Identifying_data_tests.Born_dates_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 10, iterationCount: 10)]
public class IdentifyingDataBornDatesRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _name = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptBornDatesRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _name = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public byte[] EncryptBornDatesRsa()
    {
        return _rsa.Encrypt(_name);
    }

    [GlobalSetup(Target = nameof(DecryptBornDatesRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
        _name = _rsa.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptBornDatesRsa() => _rsa.Decrypt(_name);
}