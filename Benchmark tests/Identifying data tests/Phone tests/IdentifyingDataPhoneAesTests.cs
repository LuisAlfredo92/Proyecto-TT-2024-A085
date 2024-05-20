using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Identifying_data.Phone_numbers;
using Aes = BlockCiphers.Aes;

namespace Phone_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class IdentifyingDataPhoneAesTests
{
    private Aes _aes = null!;
    private byte[] _phone = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptPhonesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _phone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
    }

    [Benchmark]
    public byte[] EncryptPhonesAes() => _aes.Encrypt(_phone, out _);

    [GlobalSetup(Target = nameof(DecryptPhonesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedPhone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        _phone = _aes.Encrypt(generatedPhone, out _tag);
    }

    [Benchmark]
    public byte[] DecryptPhonesAes() => _aes.Decrypt(_phone, _tag);
}