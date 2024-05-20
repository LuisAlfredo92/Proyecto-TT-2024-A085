using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Identifying_data.Exterior_numbers;
using Identifying_data.Municipalities;
using Identifying_data.Postal_code;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using Stream_ciphers;

namespace Addresses_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataAddressChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _address = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptNamesChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        _address = Encoding.UTF8.GetBytes(address);
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_address);
    }

    [GlobalSetup(Target = nameof(DecryptNamesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        var generatedName = Encoding.UTF8.GetBytes(address);
        _address = _chaCha20.Encrypt(generatedName);
    }

    [Benchmark]
    public byte[] DecryptNamesChaCha20() => _chaCha20.Decrypt(_address);
}