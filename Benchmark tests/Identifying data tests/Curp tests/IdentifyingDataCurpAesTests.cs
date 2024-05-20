using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Identifying_data.Curps;
using Aes = BlockCiphers.Aes;

namespace Curp_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpAesTests
{
    private Aes _aes = null!;
    private byte[] _curp = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptNamesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptNamesAes() => _aes.Encrypt(_curp, out _);

    [GlobalSetup(Target = nameof(DecryptNamesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedCurp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
        _curp = _aes.Encrypt(generatedCurp, out _tag);
    }

    [Benchmark]
    public byte[] DecryptNamesAes() => _aes.Decrypt(_curp, _tag);
}