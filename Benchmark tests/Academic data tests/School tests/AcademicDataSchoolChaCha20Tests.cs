using System.Security.Cryptography;
using System.Text;
using Academic_data.Cct;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace School_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataSchoolChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _school = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptSchoolChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _school = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSchoolChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_school);
    }

    [GlobalSetup(Target = nameof(DecryptSchoolChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedSchool = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
        _school = _chaCha20.Encrypt(generatedSchool);
    }

    [Benchmark]
    public byte[] DecryptSchoolChaCha20() => _chaCha20.Decrypt(_school);
}