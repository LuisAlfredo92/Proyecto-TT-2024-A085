﻿using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _passportId = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPassportIdCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _passportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPassportIdCast256Fpe() => _cast256Fpe.Encrypt(_passportId);

    [GlobalSetup(Target = nameof(DecryptPassportIdCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedPassportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
        _passportId = _cast256Fpe.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public char[] DecryptPassportIdCast256Fpe() => _cast256Fpe.Decrypt(_passportId);
}