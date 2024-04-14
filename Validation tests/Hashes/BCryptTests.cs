using System.Security.Cryptography;
using Hashes;

namespace Validation_tests.Hashes;

public class BCryptTests
{
    [Fact]
    public void TestCase1()
    {
        byte[] plainData = [],
            salt = RandomNumberGenerator.GetBytes(16);
        var cost = 4;
        var hash = new BCrypt(salt, cost).Hash(plainData);
    }
}