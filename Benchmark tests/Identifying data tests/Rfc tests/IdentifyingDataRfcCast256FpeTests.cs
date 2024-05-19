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
public class IdentifyingDataRfcCast256FpeTests
{
    private Cast256Fpe _serpentFpe = null!;
    private char[] _rfc = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptRfcCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _rfc = RfcGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptRfcCast256Fpe() => _serpentFpe.Encrypt(_rfc);

    [GlobalSetup(Target = nameof(DecryptRfcCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedName = RfcGenerator.Generate().ToCharArray();
        _rfc = _serpentFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptRfcCast256Fpe() => _serpentFpe.Decrypt(_rfc);
}