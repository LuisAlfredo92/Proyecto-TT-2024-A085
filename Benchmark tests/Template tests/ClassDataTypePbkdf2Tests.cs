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
public class ClassDataTypePbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _yourData = null!;

    [GlobalSetup(Target = nameof(EncryptCurpPbkdf2))]
    public void SetupEncryption()
    {
        _yourData = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpPbkdf2() => _argon2Id.Hash(_yourData);
}