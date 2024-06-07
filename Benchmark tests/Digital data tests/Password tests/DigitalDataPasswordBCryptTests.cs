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
public class DigitalDataPasswordBCryptTests
{
    private readonly BCrypt _bCrypt = new(cost:10);
    private byte[] _password = null!;

    [GlobalSetup(Target = nameof(EncryptPasswordBCrypt))]
    public void SetupEncryption()
    {
        _password = Encoding.UTF8.GetBytes(PasswordsGenerator.GeneratePassword());
    }

    [Benchmark]
    public Span<byte> EncryptPasswordBCrypt() => _bCrypt.Hash(_password);
}