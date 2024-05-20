using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Exterior_numbers;
using Identifying_data.Municipalities;
using Identifying_data.Postal_code;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;

namespace Addresses_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataAddressCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _address = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptNamesCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        _address = Encoding.UTF8.GetBytes(address);
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_address);
    }

    [GlobalSetup(Target = nameof(DecryptNamesCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        var generatedDate = Encoding.UTF8.GetBytes(address);
        _address = _camellia.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptNamesCamellia() => _camellia.Decrypt(_address);
}