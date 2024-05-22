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
public class DigitalDataPasswordPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _password = null!;

    [GlobalSetup(Target = nameof(EncryptPasswordPbkdf2))]
    public void SetupEncryption()
    {
        _password = Encoding.UTF8.GetBytes(PasswordsGenerator.GeneratePassword());
    }

    [Benchmark]
    public Span<byte> EncryptPasswordPbkdf2() => _argon2Id.Hash(_password);
}