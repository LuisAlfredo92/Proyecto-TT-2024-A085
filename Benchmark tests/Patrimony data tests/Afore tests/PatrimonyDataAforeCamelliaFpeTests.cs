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
public class PatrimonyDataAforeCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _afore = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAforeCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _afore = AforeGenerator.GenerateAforeName().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAforeCamelliaFpe() => _camelliaFpe.Encrypt(_afore);

    [GlobalSetup(Target = nameof(DecryptAforeCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedAfore = AforeGenerator.GenerateAforeName().ToCharArray();
        _afore = _camelliaFpe.Encrypt(generatedAfore);
    }

    [Benchmark]
    public char[] DecryptAforeCamelliaFpe() => _camelliaFpe.Decrypt(_afore);
}