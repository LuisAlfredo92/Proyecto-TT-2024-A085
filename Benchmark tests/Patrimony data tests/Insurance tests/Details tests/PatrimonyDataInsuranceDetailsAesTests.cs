using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using General_Data;
using Aes = BlockCiphers.Aes;

namespace Details_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceDetailsAesTests
{
    private Aes _aes = null!;
    private byte[] _insuranceDetails = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceDetailsAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _insuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
    }

    [Benchmark]
    public byte[] EncryptInsuranceDetailsAes() => _aes.Encrypt(_insuranceDetails, out _);

    [GlobalSetup(Target = nameof(DecryptInsuranceDetailsAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedInsuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
        _insuranceDetails = _aes.Encrypt(generatedInsuranceDetails, out _tag);
    }

    [Benchmark]
    public byte[] DecryptInsuranceDetailsAes() => _aes.Decrypt(_insuranceDetails, _tag);
}