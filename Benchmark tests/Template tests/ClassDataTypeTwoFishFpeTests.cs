using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Curps;

namespace Tests.Identifying_data_tests.CURP_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _yourData = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTypeTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _yourData = [DataGeneratorCharArray];
    }

    [Benchmark]
    public char[] EncryptTypeTwoFishFpe() => _twoFishFpe.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedType = [DataGeneratorCharArray];
        _yourData = _twoFishFpe.Encrypt(generatedType);
    }

    [Benchmark]
    public char[] DecryptTypeTwoFishFpe() => _twoFishFpe.Decrypt(_yourData);
}