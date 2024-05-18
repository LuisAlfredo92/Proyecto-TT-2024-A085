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
public class IdentifyingDataCurpCamelliaFpeTests
{
    private CamelliaFpe _serpentFpe = null!;
    private char[] _curp = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCurpsCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _curp = CurpsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCurpsCamelliaFpe() => _serpentFpe.Encrypt(_curp);

    [GlobalSetup(Target = nameof(DecryptCurpsCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedName = CurpsGenerator.Generate().ToCharArray();
        _curp = _serpentFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptCurpsCamelliaFpe() => _serpentFpe.Decrypt(_curp);
}