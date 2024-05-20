using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Identifying_data.Military_service_number;
using Aes = BlockCiphers.Aes;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberAesTests
{
    private Aes _aes = null!;
    private byte[] _serviceNumber = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _serviceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
    }

    [Benchmark]
    public byte[] EncryptMillitaryServiceNumberAes() => _aes.Encrypt(_serviceNumber, out _);

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedMillitaryServiceNumber = Encoding.UTF8.GetBytes(MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber());
        _serviceNumber = _aes.Encrypt(generatedMillitaryServiceNumber, out _tag);
    }

    [Benchmark]
    public byte[] DecryptMillitaryServiceNumberAes() => _aes.Decrypt(_serviceNumber, _tag);
}