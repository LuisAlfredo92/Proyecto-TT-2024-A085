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
public class ClassDataTypeAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _yourData = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTypeAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _yourData = [DataGeneratorCharArray];
    }

    [Benchmark]
    public char[] EncryptTypeAesFpe() => _aesFpe.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedType = [DataGeneratorCharArray];
        _yourData = _aesFpe.Encrypt(generatedType);
    }

    [Benchmark]
    public char[] DecryptTypeAesFpe() => _aesFpe.Decrypt(_yourData);
}