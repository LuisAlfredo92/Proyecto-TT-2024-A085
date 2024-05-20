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
public class IdentifyingDataRfcAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _rfc = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptRfcAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _rfc = RfcGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptRfcAesFpe() => _aesFpe.Encrypt(_rfc);

    [GlobalSetup(Target = nameof(DecryptRfcAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedName = RfcGenerator.Generate().ToCharArray();
        _rfc = _aesFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptRfcAesFpe() => _aesFpe.Decrypt(_rfc);
}