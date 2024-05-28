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
public class AcademicDataEnrolmentPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _enrolment = null!;

    [GlobalSetup(Target = nameof(EncryptEnrolmentPbkdf2))]
    public void SetupEncryption()
    {
        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptEnrolmentPbkdf2() => _argon2Id.Hash(_enrolment);
}