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
public class ClassDataTypeCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _yourData = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTypeCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _yourData = [DataGeneratorCharArray];
    }

    [Benchmark]
    public char[] EncryptTypeCast256Fpe() => _cast256Fpe.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedType = [DataGeneratorCharArray];
        _yourData = _cast256Fpe.Encrypt(generatedType);
    }

    [Benchmark]
    public char[] DecryptTypeCast256Fpe() => _cast256Fpe.Decrypt(_yourData);
}