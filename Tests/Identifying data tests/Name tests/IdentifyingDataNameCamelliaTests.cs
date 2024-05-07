using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Names;
using System.Security.Cryptography;
using System.Text;

namespace Tests.Identifying_data_tests.Name_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataNameCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _name = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptNamesCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _name = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_name);
    }

    [GlobalSetup(Target = nameof(DecryptNamesCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedName = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
        _name = _camellia.Encrypt(generatedName);
    }

    [Benchmark]
    public byte[] DecryptNamesCamellia() => _camellia.Decrypt(_name);
}