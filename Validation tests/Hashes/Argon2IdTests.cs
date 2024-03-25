using Hashes;

namespace Validation_tests.Hashes;

public class Argon2IdTests
{
    [Fact]
    public void TestCase1()
    {
        const int memory = 32,
            passes = 3,
            parallelism = 4;
        byte[] password = new byte[32],
            salt = new byte[16],
            secret = new byte[8],
            associatedData = new byte[12],
            expectedResult =
            [
                0x0d, 0x64, 0x0d, 0xf5, 0x8d, 0x78, 0x76, 0x6c, 0x08, 0xc0, 0x37, 0xa3, 0x4a, 0x8b, 0x53, 0xc9, 0xd0,
                0x1e, 0xf0, 0x45, 0x2d, 0x75, 0xb6, 0x5e, 0xb5, 0x25, 0x20, 0xe9, 0x6b, 0x01, 0xe6, 0x59
            ];

        Array.Fill<byte>(password, 1);
        Array.Fill<byte>(salt, 2);
        Array.Fill<byte>(secret, 3);
        Array.Fill<byte>(associatedData, 4);

        Argon2Id argon2Id = new(memory, passes, parallelism, salt, secret, associatedData);
        var result = argon2Id.Hash(password);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TestCase2()
    {
        var password = new byte[32];
        Random.Shared.NextBytes(password);

        Argon2Id argon2Id = new();
        var result = argon2Id.Hash(password);

        Assert.True(result.Length == 32);
    }
}