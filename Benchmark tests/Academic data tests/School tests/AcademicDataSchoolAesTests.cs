using System.Security.Cryptography;
using System.Text;
using Academic_data.Cct;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace School_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataSchoolAesTests
{
    private Aes _aes = null!;
    private byte[] _school = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptSchoolAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _school = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
    }

    [Benchmark]
    public byte[] EncryptSchoolAes() => _aes.Encrypt(_school, out _);

    [GlobalSetup(Target = nameof(DecryptSchoolAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedSchool = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
        _school = _aes.Encrypt(generatedSchool, out _tag);
    }

    [Benchmark]
    public byte[] DecryptSchoolAes() => _aes.Decrypt(_school, _tag);
}