using System.Security.Cryptography;
using Hashes;

namespace Validation_tests.Ciphers.Hashes;

public class BCryptTests
{
    [Fact]
    public void TestCase1()
    {
        var plainData = new byte[Random.Shared.Next(72)];
        var hash = new BCrypt().Hash(plainData);
        Assert.True(hash.Length == 24);
    }

    [Fact]
    public void TestCase2()
    {
        byte[] plainData = [],
        salt = RandomNumberGenerator.GetBytes(16);
        var cost = 4;
        var hash = new BCrypt(salt, cost).Hash(plainData);
    }
}