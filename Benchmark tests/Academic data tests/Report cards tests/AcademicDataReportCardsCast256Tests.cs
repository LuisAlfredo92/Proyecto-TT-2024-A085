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
public class AcademicDataReportCardCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _reportCard = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptReportCardCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _reportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReportCardCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_reportCard);
    }

    [GlobalSetup(Target = nameof(DecryptReportCardCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedReportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
        _reportCard = _cast256.Encrypt(generatedReportCard);
    }

    [Benchmark]
    public byte[] DecryptReportCardCast256() => _cast256.Decrypt(_reportCard);
}