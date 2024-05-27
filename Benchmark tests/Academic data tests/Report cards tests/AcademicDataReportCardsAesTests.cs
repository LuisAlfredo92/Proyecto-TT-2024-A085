using System.Security.Cryptography;
using Academic_data.Report_cards;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Report_cards_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataReportCardAesTests
{
    private Aes _aes = null!;
    private byte[] _reportCard = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptReportCardAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _reportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptReportCardAes() => _aes.Encrypt(_reportCard, out _);

    [GlobalSetup(Target = nameof(DecryptReportCardAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedReportCard = File.ReadAllBytes(ReportCardsGenerator.Generate());
        _reportCard = _aes.Encrypt(generatedReportCard, out _tag);
    }

    [Benchmark]
    public byte[] DecryptReportCardAes() => _aes.Decrypt(_reportCard, _tag);
}