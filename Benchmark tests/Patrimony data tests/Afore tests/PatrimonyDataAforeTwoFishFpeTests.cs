using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Afore;

namespace Afore_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _afore = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAforeTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _afore = AforeGenerator.GenerateAforeName().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAforeTwoFishFpe() => _twoFishFpe.Encrypt(_afore);

    [GlobalSetup(Target = nameof(DecryptAforeTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedAfore = AforeGenerator.GenerateAforeName().ToCharArray();
        _afore = _twoFishFpe.Encrypt(generatedAfore);
    }

    [Benchmark]
    public char[] DecryptAforeTwoFishFpe() => _twoFishFpe.Decrypt(_afore);
}