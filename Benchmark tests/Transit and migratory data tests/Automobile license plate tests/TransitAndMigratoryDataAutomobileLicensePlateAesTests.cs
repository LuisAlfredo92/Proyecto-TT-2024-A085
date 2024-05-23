using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Automobile_license_plate;
using Aes = BlockCiphers.Aes;

namespace Automobile_license_plate_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataAutomobileLicensePlateAesTests
{
    private Aes _aes = null!;
    private byte[] _automobileLicensePlate = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public byte[] EncryptAutomobileLicensePlateAes() => _aes.Encrypt(_automobileLicensePlate, out _);

    [GlobalSetup(Target = nameof(DecryptAutomobileLicensePlateAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedAutomobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
        _automobileLicensePlate = _aes.Encrypt(generatedAutomobileLicensePlate, out _tag);
    }

    [Benchmark]
    public byte[] DecryptAutomobileLicensePlateAes() => _aes.Decrypt(_automobileLicensePlate, _tag);
}