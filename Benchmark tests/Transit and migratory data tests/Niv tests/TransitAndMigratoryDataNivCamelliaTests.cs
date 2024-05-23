using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Transit_and_migratory_data.Niv;

namespace Niv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataNivCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _niv = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptNivCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _niv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNivCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_niv);
    }

    [GlobalSetup(Target = nameof(DecryptNivCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedNiv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
        _niv = _camellia.Encrypt(generatedNiv);
    }

    [Benchmark]
    public byte[] DecryptNivCamellia() => _camellia.Decrypt(_niv);
}