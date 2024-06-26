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
public class ClassDataTypeBlake3Tests
{
    private byte[] _yourData = null!;

    [GlobalSetup(Target = nameof(EncryptTypeBlake3))]
    public void SetupEncryption()
    {
        _yourData = [DataGeneratorBytes];
    }

    [Benchmark]
    public Span<byte> EncryptTypeBlake3() => Blake3.Hash(_yourData);
}