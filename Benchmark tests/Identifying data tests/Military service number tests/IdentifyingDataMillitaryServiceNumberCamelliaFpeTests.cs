using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Curps;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberCamelliaFpeTests
{
    private CamelliaFpe _serpentFpe = null!;
    private char[] _serviceNumber = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _serviceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptMillitaryServiceNumberCamelliaFpe() => _serpentFpe.Encrypt(_serviceNumber);

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedMillitaryServiceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
        _serviceNumber = _serpentFpe.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public char[] DecryptMillitaryServiceNumberCamelliaFpe() => _serpentFpe.Decrypt(_serviceNumber);
}