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
public class ClassDataTypeCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _yourData = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTypeCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _yourData = [DataGeneratorCharArray];
    }

    [Benchmark]
    public char[] EncryptTypeCamelliaFpe() => _camelliaFpe.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedType = [DataGeneratorCharArray];
        _yourData = _camelliaFpe.Encrypt(generatedType);
    }

    [Benchmark]
    public char[] DecryptTypeCamelliaFpe() => _camelliaFpe.Decrypt(_yourData);
}