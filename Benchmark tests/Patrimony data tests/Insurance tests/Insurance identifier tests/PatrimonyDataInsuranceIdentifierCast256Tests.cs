using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptInsuranceIdentifierCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceIdentifierCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_insuranceIdentifier);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedInsuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
        _insuranceIdentifier = _cast256.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public byte[] DecryptInsuranceIdentifierCast256() => _cast256.Decrypt(_insuranceIdentifier);
}