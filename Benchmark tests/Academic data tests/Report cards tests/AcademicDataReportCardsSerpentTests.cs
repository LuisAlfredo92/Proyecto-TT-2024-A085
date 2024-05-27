using System.Security.Cryptography;
using Academic_data.Report_cards;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Report_cards_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataReportCardSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _reportCard = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptReportCardSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _reportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReportCardSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_reportCard);
    }

    [GlobalSetup(Target = nameof(DecryptReportCardSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedReportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
        _reportCard = _serpent.Encrypt(generatedReportCard);
    }

    [Benchmark]
    public byte[] DecryptReportCardSerpent() => _serpent.Decrypt(_reportCard);
}