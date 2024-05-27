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
public class AcademicDataSchoolSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _school = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptSchoolSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _school = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSchoolSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_school);
    }

    [GlobalSetup(Target = nameof(DecryptSchoolSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedSchool = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
        _school = _serpent.Encrypt(generatedSchool);
    }

    [Benchmark]
    public byte[] DecryptSchoolSerpent() => _serpent.Decrypt(_school);
}