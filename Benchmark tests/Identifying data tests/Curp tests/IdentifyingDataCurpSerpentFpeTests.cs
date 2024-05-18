using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Curps;

namespace Curp_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _curp = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCurpsSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _curp = CurpsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCurpsSerpentFpe() => _serpentFpe.Encrypt(_curp);

    [GlobalSetup(Target = nameof(DecryptCurpsSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedName = CurpsGenerator.Generate().ToCharArray();
        _curp = _serpentFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptCurpsSerpentFpe() => _serpentFpe.Decrypt(_curp);
}