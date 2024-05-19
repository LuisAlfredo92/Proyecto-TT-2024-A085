using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Rfc;

namespace Rfc_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataRfcSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _rfc = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptRfcSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _rfc = RfcGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptRfcSerpentFpe() => _serpentFpe.Encrypt(_rfc);

    [GlobalSetup(Target = nameof(DecryptRfcSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedName = RfcGenerator.Generate().ToCharArray();
        _rfc = _serpentFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptRfcSerpentFpe() => _serpentFpe.Decrypt(_rfc);
}