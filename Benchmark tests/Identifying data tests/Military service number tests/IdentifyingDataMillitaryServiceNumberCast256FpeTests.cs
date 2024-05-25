using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberCast256FpeTests
{
    private Cast256Fpe _serpentFpe = null!;
    private char[] _serviceNumber = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _serviceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptMillitaryServiceNumberCast256Fpe() => _serpentFpe.Encrypt(_serviceNumber);

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedMillitaryServiceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
        _serviceNumber = _serpentFpe.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public char[] DecryptMillitaryServiceNumberCast256Fpe() => _serpentFpe.Decrypt(_serviceNumber);
}