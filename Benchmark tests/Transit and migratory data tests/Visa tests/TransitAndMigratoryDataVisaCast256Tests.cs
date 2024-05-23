using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Transit_and_migratory_data.Visa;

namespace Visa_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVisaCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _visa = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptVisaCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _visa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptVisaCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_visa);
    }

    [GlobalSetup(Target = nameof(DecryptVisaCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedVisa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
        _visa = _cast256.Encrypt(generatedVisa);
    }

    [Benchmark]
    public byte[] DecryptVisaCast256() => _cast256.Decrypt(_visa);
}