using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Names;
using System.Security.Cryptography;
using System.Text;
using FPE_ciphers;
using Stream_ciphers;

namespace Tests.Identifying_data_tests.Name_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 10, iterationCount: 10)]
public class IdentifyingDataNameTwoFishFpeTests
{
    private TwoFishFpe _chaCha20 = null!;
    private char[] _name = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ. áéíóúÁÉÍÓÚ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNamesTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _chaCha20 = new TwoFishFpe(_key.AsSpan(), _alphabet);

        _name = NamesGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNamesTwoFishFpe() => _chaCha20.Encrypt(_name);

    [GlobalSetup(Target = nameof(DecryptNamesTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _chaCha20 = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedName = NamesGenerator.Generate().ToCharArray();
        _name = _chaCha20.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptNamesTwoFishFpe() => _chaCha20.Decrypt(_name);
}