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
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class ClassDataTypeSha3Tests
{
    private byte[] _yourData = null!;

    [GlobalSetup(Target = nameof(EncryptCurpSha3))]
    public void SetupEncryption()
    {
        _yourData = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpSha3() => Sha3.Hash(_yourData);
}