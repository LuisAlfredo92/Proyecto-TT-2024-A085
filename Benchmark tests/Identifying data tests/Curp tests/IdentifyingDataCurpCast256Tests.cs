using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Curps;

namespace Curp_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _curp = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptNamesCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_curp);
    }

    [GlobalSetup(Target = nameof(DecryptNamesCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedCurp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
        _curp = _cast256.Encrypt(generatedCurp);
    }

    [Benchmark]
    public byte[] DecryptNamesCast256() => _cast256.Decrypt(_curp);
}