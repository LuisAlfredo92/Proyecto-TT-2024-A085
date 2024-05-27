﻿using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Health_data.Clinic_historical;

namespace Clinic_historical_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicHistoricalSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _clinicHistorical = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptClinicHistoricalSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _clinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicHistoricalSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_clinicHistorical);
    }

    [GlobalSetup(Target = nameof(DecryptClinicHistoricalSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedClinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
        _clinicHistorical = _serpent.Encrypt(generatedClinicHistorical);
    }

    [Benchmark]
    public byte[] DecryptClinicHistoricalSerpent() => _serpent.Decrypt(_clinicHistorical);
}