using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Afore;

namespace Afore_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _afore = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptAforeCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _afore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAforeCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_afore);
    }

    [GlobalSetup(Target = nameof(DecryptAforeCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedAfore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
        _afore = _camellia.Encrypt(generatedAfore);
    }

    [Benchmark]
    public byte[] DecryptAforeCamellia() => _camellia.Decrypt(_afore);
}