using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Identifying_data.Phone_numbers;
using Stream_ciphers;

namespace Phone_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class IdentifyingDataPhoneChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _phone = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptPhonesChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _phone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPhonesChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_phone);
    }

    [GlobalSetup(Target = nameof(DecryptPhonesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedPhone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        _phone = _chaCha20.Encrypt(generatedPhone);
    }

    [Benchmark]
    public byte[] DecryptPhonesChaCha20() => _chaCha20.Decrypt(_phone);
}