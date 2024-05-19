﻿using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Curps;

namespace Tests.Identifying_data_tests.CURP_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class ClassDataTypeCast256FpeTests
{
    private Cast256Fpe _serpentFpe = null!;
    private char[] _yourData = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCurpsCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _yourData = CurpsGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCurpsCast256Fpe() => _serpentFpe.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptCurpsCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedName = CurpsGenerator.Generate().ToCharArray();
        _yourData = _serpentFpe.Encrypt(generatedName);
    }

    [Benchmark]
    public char[] DecryptCurpsCast256Fpe() => _serpentFpe.Decrypt(_yourData);
}