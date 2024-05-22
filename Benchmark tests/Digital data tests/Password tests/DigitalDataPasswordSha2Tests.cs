using System.Text;
using BenchmarkDotNet.Attributes;
using Digital_data.Passwords;
using Hashes;

namespace Password_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataPasswordSha2Tests
{
    private byte[] _password = null!;

    [GlobalSetup(Target = nameof(EncryptPasswordSha2))]
    public void SetupEncryption()
    {
        _password = Encoding.UTF8.GetBytes(PasswordsGenerator.GeneratePassword());
    }

    [Benchmark]
    public Span<byte> EncryptPasswordSha2() => Sha2.Hash(_password);
}