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
public class AcademicDataSchoolTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _school = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptSchoolTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _school = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSchoolTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_school);
    }

    [GlobalSetup(Target = nameof(DecryptSchoolTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedSchool = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
        _school = _twoFish.Encrypt(generatedSchool);
    }

    [Benchmark]
    public byte[] DecryptSchoolTwoFish() => _twoFish.Decrypt(_school);
}