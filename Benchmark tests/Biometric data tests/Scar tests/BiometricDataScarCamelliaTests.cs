using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Scars;
using BlockCiphers;

namespace Scar_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataScarCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _scar = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptScarCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _scar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptScarCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_scar);
    }

    [GlobalSetup(Target = nameof(DecryptScarCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedScar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
        _scar = _camellia.Encrypt(generatedScar);
    }

    [Benchmark]
    public byte[] DecryptScarCamellia() => _camellia.Decrypt(_scar);
}