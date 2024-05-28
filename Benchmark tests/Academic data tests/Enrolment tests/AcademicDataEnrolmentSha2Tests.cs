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
public class AcademicDataEnrolmentSha2Tests
{
    private byte[] _enrolment = null!;

    [GlobalSetup(Target = nameof(EncryptEnrolmentSha2))]
    public void SetupEncryption()
    {
        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptEnrolmentSha2() => Sha2.Hash(_enrolment);
}