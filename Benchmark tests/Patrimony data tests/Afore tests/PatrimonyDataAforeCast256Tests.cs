using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Afore;

namespace Afore_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _afore = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptAforeCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _afore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAforeCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_afore);
    }

    [GlobalSetup(Target = nameof(DecryptAforeCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedAfore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
        _afore = _cast256.Encrypt(generatedAfore);
    }

    [Benchmark]
    public byte[] DecryptAforeCast256() => _cast256.Decrypt(_afore);
}