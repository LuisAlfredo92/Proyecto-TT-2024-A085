using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Identifying_data.Exterior_numbers;
using Identifying_data.Municipalities;
using Identifying_data.Postal_code;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using Aes = BlockCiphers.Aes;

namespace Addresses_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataAddressAesTests
{
    private Aes _aes = null!;
    private byte[] _address = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptNamesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        _address = Encoding.UTF8.GetBytes(address);
    }

    [Benchmark]
    public byte[] EncryptNamesAes() => _aes.Encrypt(_address, out _);

    [GlobalSetup(Target = nameof(DecryptNamesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var address = $"{StreetNamesGenerator.Generate()} {ExteriorNumbersGenerator.GenerateHouseNumber()}, {SettlementsGenerator.Generate()}, {MunicipalitiesGenerator.Generate()} {PostalCodeGenerator.Generate()}, {StatesGenerator.Generate()}";
        var generatedDate = Encoding.UTF8.GetBytes(address);
        _address = _aes.Encrypt(generatedDate, out _tag);
    }

    [Benchmark]
    public byte[] DecryptNamesAes() => _aes.Decrypt(_address, _tag);
}