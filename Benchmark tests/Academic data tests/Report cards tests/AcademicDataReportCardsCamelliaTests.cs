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
public class AcademicDataReportCardCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _reportCard = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptReportCardCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _reportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReportCardCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_reportCard);
    }

    [GlobalSetup(Target = nameof(DecryptReportCardCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedReportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
        _reportCard = _camellia.Encrypt(generatedReportCard);
    }

    [Benchmark]
    public byte[] DecryptReportCardCamellia() => _camellia.Decrypt(_reportCard);
}