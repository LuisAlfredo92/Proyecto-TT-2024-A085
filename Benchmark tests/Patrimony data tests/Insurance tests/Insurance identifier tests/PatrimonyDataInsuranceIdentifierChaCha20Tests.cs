using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptInsuranceIdentifierChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceIdentifierChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_insuranceIdentifier);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedInsuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
        _insuranceIdentifier = _chaCha20.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public byte[] DecryptInsuranceIdentifierChaCha20() => _chaCha20.Decrypt(_insuranceIdentifier);
}