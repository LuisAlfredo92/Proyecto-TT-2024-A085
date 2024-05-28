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
public class AcademicDataEnrolmentBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _enrolment = null!;

    [GlobalSetup(Target = nameof(EncryptEnrolmentBCrypt))]
    public void SetupEncryption()
    {
        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptEnrolmentBCrypt() => _argon2Id.Hash(_enrolment);
}