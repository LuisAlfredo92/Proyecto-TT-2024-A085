﻿using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Niv;

namespace Niv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataNivCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _niv = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNivCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _niv = NivGenerator.GenerateNiv().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNivCamelliaFpe() => _camelliaFpe.Encrypt(_niv);

    [GlobalSetup(Target = nameof(DecryptNivCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedNiv = NivGenerator.GenerateNiv().ToCharArray();
        _niv = _camelliaFpe.Encrypt(generatedNiv);
    }

    [Benchmark]
    public char[] DecryptNivCamelliaFpe() => _camelliaFpe.Decrypt(_niv);
}