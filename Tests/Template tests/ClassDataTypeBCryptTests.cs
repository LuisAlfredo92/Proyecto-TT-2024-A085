using BenchmarkDotNet.Attributes;
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
public class ClassDataTypeBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _yourData = null!;

    [GlobalSetup(Target = nameof(EncryptCurpBCrypt))]
    public void SetupEncryption()
    {
        _yourData = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpBCrypt() => _argon2Id.Hash(_yourData);
}