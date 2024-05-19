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
public class IdentifyingDataRfcTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _rfc = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptRfcTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _rfc = RfcGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptRfcTwoFishFpe() => _twoFishFpe.Encrypt(_rfc);

    [GlobalSetup(Target = nameof(DecryptRfcTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedName = RfcGenerator.Generate().ToCharArray();
        _rfc = _twoFishFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptRfcTwoFishFpe() => _twoFishFpe.Decrypt(_rfc);
}