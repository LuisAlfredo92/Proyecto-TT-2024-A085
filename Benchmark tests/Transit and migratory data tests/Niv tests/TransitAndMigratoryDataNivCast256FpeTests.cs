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
public class TransitAndMigratoryDataNivCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _niv = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNivCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _niv = NivGenerator.GenerateNiv().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNivCast256Fpe() => _cast256Fpe.Encrypt(_niv);

    [GlobalSetup(Target = nameof(DecryptNivCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedNiv = NivGenerator.GenerateNiv().ToCharArray();
        _niv = _cast256Fpe.Encrypt(generatedNiv);
    }

    [Benchmark]
    public char[] DecryptNivCast256Fpe() => _cast256Fpe.Decrypt(_niv);
}