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
public class IdentifyingDataMillitaryServiceNumberAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _serviceNumber = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _serviceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptMillitaryServiceNumberAesFpe() => _aesFpe.Encrypt(_serviceNumber);

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedMillitaryServiceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
        _serviceNumber = _aesFpe.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public char[] DecryptMillitaryServiceNumberAesFpe() => _aesFpe.Decrypt(_serviceNumber);
}