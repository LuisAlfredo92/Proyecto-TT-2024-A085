using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Niv;

namespace Niv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataNivTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _niv = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNivTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _niv = NivGenerator.GenerateNiv().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNivTwoFishFpe() => _twoFishFpe.Encrypt(_niv);

    [GlobalSetup(Target = nameof(DecryptNivTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedNiv = NivGenerator.GenerateNiv().ToCharArray();
        _niv = _twoFishFpe.Encrypt(generatedNiv);
    }

    [Benchmark]
    public char[] DecryptNivTwoFishFpe() => _twoFishFpe.Decrypt(_niv);
}