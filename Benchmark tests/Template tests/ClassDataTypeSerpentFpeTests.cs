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
public class ClassDataTypeSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _yourData = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTypeSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _yourData = CurpsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptTypeSerpentFpe() => _serpentFpe.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedType = CurpsGenerator.Generate().ToCharArray();
        _yourData = _serpentFpe.Encrypt(generatedType);
    }

    [Benchmark]
    public char[] DecryptTypeSerpentFpe() => _serpentFpe.Decrypt(_yourData);
}