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
public class AcademicDataDegreeCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _degree = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptDegreeCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _degree = File.ReadAllBytes(DegreeGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptDegreeCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_degree);
    }

    [GlobalSetup(Target = nameof(DecryptDegreeCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedDegree = File.ReadAllBytes(DegreeGenerator.Generate());
        _degree = _cast256.Encrypt(generatedDegree);
    }

    [Benchmark]
    public byte[] DecryptDegreeCast256() => _cast256.Decrypt(_degree);
}