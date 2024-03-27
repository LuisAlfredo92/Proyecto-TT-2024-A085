using System.Text;
using Hashes;

namespace Validation_tests.Hashes;

public class BCryptTests
{
    [Fact]
    public void TestCase1()
    {
        var plainData = new byte[Random.Shared.Next(72)];
        var hash = new BCrypt().Hash(plainData);
        Assert.True(hash.Length == 24);
    }
}