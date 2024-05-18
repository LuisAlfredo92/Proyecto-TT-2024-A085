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
public class IdentifyingDataAddressTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _address = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptTypeTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        _address = Encoding.UTF8.GetBytes(address);
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTypeTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_address);
    }

    [GlobalSetup(Target = nameof(DecryptNamesTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        var generatedDate = Encoding.UTF8.GetBytes(address);
        _address = _twoFish.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptNamesTwoFish() => _twoFish.Decrypt(_address);
}