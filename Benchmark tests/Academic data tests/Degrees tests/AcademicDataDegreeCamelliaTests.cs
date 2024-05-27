using System.Security.Cryptography;
using Academic_data.Degrees;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Degrees_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class AcademicDataDegreeCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _degree = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptDegreeCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _degree = File.ReadAllBytes(DegreeGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptDegreeCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_degree);
    }

    [GlobalSetup(Target = nameof(DecryptDegreeCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedDegree = File.ReadAllBytes(DegreeGenerator.Generate());
        _degree = _camellia.Encrypt(generatedDegree);
    }

    [Benchmark]
    public byte[] DecryptDegreeCamellia() => _camellia.Decrypt(_degree);
}