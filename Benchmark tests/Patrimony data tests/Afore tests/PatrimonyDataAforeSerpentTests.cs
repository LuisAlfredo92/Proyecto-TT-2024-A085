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
public class PatrimonyDataAforeSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _afore = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptAforeSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _afore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAforeSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_afore);
    }

    [GlobalSetup(Target = nameof(DecryptAforeSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedAfore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
        _afore = _serpent.Encrypt(generatedAfore);
    }

    [Benchmark]
    public byte[] DecryptAforeSerpent() => _serpent.Decrypt(_afore);
}