using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Identifying_data.Born_dates;
using Stream_ciphers;

namespace Tests.Identifying_data_tests.Born_dates_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 10, iterationCount: 10)]
public class IdentifyingDataBornDatesChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _bornDates = null!;
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

        _bornDates = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
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
        return _chaCha20.Encrypt(_bornDates);
    }

    [GlobalSetup(Target = nameof(DecryptNamesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedName = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDates = _chaCha20.Encrypt(generatedName);
    }

    [Benchmark]
    public byte[] DecryptNamesChaCha20() => _chaCha20.Decrypt(_bornDates);
}