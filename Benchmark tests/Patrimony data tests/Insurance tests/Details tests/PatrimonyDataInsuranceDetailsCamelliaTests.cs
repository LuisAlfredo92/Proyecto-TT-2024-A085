using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using General_Data;

namespace Details_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceDetailsCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _insuranceDetails = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptInsuranceDetailsCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _insuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceDetailsCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_insuranceDetails);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceDetailsCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedInsuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
        _insuranceDetails = _camellia.Encrypt(generatedInsuranceDetails);
    }

    [Benchmark]
    public byte[] DecryptInsuranceDetailsCamellia() => _camellia.Decrypt(_insuranceDetails);
}