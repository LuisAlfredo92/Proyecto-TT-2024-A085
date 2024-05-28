using System.Text;
using Academic_data.Enrolment;
using BenchmarkDotNet.Attributes;
using Hashes;

namespace Enrolment_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataEnrolmentArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _enrolment = null!;

    [GlobalSetup(Target = nameof(EncryptEnrolmentArgon2Id))]
    public void SetupEncryption()
    {
        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptEnrolmentArgon2Id() => _argon2Id.Hash(_enrolment);
}