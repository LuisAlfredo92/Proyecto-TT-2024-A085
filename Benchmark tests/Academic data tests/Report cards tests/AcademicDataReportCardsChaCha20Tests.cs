using System.Security.Cryptography;
using Academic_data.Report_cards;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Report_cards_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataReportCardChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _reportCard = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptReportCardChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _reportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReportCardChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_reportCard);
    }

    [GlobalSetup(Target = nameof(DecryptReportCardChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedReportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
        _reportCard = _chaCha20.Encrypt(generatedReportCard);
    }

    [Benchmark]
    public byte[] DecryptReportCardChaCha20() => _chaCha20.Decrypt(_reportCard);
}