using System.Security.Cryptography;
using System.Text;
using Academic_data.Cct;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace School_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataSchoolCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _school = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptSchoolCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _school = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSchoolCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_school);
    }

    [GlobalSetup(Target = nameof(DecryptSchoolCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedSchool = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
        _school = _cast256.Encrypt(generatedSchool);
    }

    [Benchmark]
    public byte[] DecryptSchoolCast256() => _cast256.Decrypt(_school);
}