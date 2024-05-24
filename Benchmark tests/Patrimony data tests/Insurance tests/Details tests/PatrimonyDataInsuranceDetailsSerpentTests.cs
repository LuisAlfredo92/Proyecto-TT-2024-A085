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
public class PatrimonyDataInsuranceDetailsSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _insuranceDetails = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptInsuranceDetailsSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _insuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceDetailsSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_insuranceDetails);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceDetailsSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedInsuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
        _insuranceDetails = _serpent.Encrypt(generatedInsuranceDetails);
    }

    [Benchmark]
    public byte[] DecryptInsuranceDetailsSerpent() => _serpent.Decrypt(_insuranceDetails);
}