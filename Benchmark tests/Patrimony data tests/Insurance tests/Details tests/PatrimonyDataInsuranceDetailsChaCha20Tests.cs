using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using General_Data;
using Stream_ciphers;

namespace Details_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceDetailsChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _insuranceDetails = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptInsuranceDetailsChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _insuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceDetailsChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_insuranceDetails);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceDetailsChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedInsuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
        _insuranceDetails = _chaCha20.Encrypt(generatedInsuranceDetails);
    }

    [Benchmark]
    public byte[] DecryptInsuranceDetailsChaCha20() => _chaCha20.Decrypt(_insuranceDetails);
}