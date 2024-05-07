using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Identifying_data.Names;
using System.Text;
using Aes = BlockCiphers.Aes;

namespace Tests.Identifying_data_tests.Name_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataNameAesTests
{
    private Aes _aes = null!;
    private byte[] _name = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptNamesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _name = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptNamesAes() => _aes.Encrypt(_name, out _);

    [GlobalSetup(Target = nameof(DecryptNamesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedName = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
        _name = _aes.Encrypt(generatedName, out _tag);
    }

    [Benchmark]
    public byte[] DecryptNamesAes() => _aes.Decrypt(_name, _tag);
}