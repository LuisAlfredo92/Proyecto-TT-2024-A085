using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Ideological_data.Religion;

namespace Religion_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataReligionCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _religion = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptReligionCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _religion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReligionCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_religion);
    }

    [GlobalSetup(Target = nameof(DecryptReligionCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedReligion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
        _religion = _camellia.Encrypt(generatedReligion);
    }

    [Benchmark]
    public byte[] DecryptReligionCamellia() => _camellia.Decrypt(_religion);
}