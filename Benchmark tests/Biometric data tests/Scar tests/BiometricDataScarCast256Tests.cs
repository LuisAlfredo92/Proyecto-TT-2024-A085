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
public class BiometricDataScarCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _scar = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptScarCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _scar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptScarCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_scar);
    }

    [GlobalSetup(Target = nameof(DecryptScarCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedScar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
        _scar = _cast256.Encrypt(generatedScar);
    }

    [Benchmark]
    public byte[] DecryptScarCast256() => _cast256.Decrypt(_scar);
}