﻿using BenchmarkDotNet.Attributes;
using Hashes;
using Identifying_data.Curps;
using System.Text;

namespace Tests.Identifying_data_tests.CURP_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _yourData = null!;

    [GlobalSetup(Target = nameof(EncryptTypeBCrypt))]
    public void SetupEncryption()
    {
        _yourData = [DataGeneratorBytes];
    }

    [Benchmark]
    public Span<byte> EncryptTypeBCrypt() => _argon2Id.Hash(_yourData);
}