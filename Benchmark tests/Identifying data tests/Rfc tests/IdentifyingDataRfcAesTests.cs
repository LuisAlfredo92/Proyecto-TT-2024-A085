using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Identifying_data.Rfc;
using Aes = BlockCiphers.Aes;

namespace Rfc_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataRfcAesTests
{
    private Aes _aes = null!;
    private byte[] _rfc = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptRfcAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptRfcAes() => _aes.Encrypt(_rfc, out _);

    [GlobalSetup(Target = nameof(DecryptRfcAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedRfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
        _rfc = _aes.Encrypt(generatedRfc, out _tag);
    }

    [Benchmark]
    public byte[] DecryptRfcAes() => _aes.Decrypt(_rfc, _tag);
}