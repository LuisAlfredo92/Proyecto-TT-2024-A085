using System.Linq;
using System.Runtime.InteropServices;
using Hashes;

namespace Validation_tests.Hashes;

public class Sha2Tests
{
    [Fact]
    public void TestCase1()
    {
        var sha2 = new Sha2();
        var input = "abc"u8.ToArray();
        var expected =
            Convert.FromHexString(
                "ddaf35a193617aba" +
                "cc417349ae204131" +
                "12e6fa4e89a97ea2" +
                "0a9eeee64b55d39a" +
                "2192992a274fc1a8" +
                "36ba3c23a3feebbd" +
                "454d4423643ce80e" +
                "2a9ac94fa54ca49f"
                );
        var actual = sha2.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase2()
    {
        var sha2 = new Sha2();
        var input = ""u8.ToArray();
        var expected =
            Convert.FromHexString(
                "cf83e1357eefb8bd" +
                "f1542850d66d8007" +
                "d620e4050b5715dc" +
                "83f4a921d36ce9ce" +
                "47d0d13c5d85f2b0" +
                "ff8318d2877eec2f" +
                "63b931bd47417a81" +
                "a538327af927da3e"
                );
        var actual = sha2.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase3()
    {
        var sha2 = new Sha2();
        var input = "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq"u8.ToArray();
        var expected =
            Convert.FromHexString(
                "204a8fc6dda82f0a" +
                "0ced7beb8e08a416" +
                "57c16ef468b228a8" +
                "279be331a703c335" +
                "96fd15c13b1b07f9" +
                "aa1d3bea57789ca0" +
                "31ad85c7a71dd703" +
                "54ec631238ca3445"
                );
        var actual = sha2.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase5()
    {
        var sha2 = new Sha2();
        Span<byte> bytesToRepeat = stackalloc byte[64];
        "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmno"u8.ToArray().CopyTo(bytesToRepeat);
        var input = new byte[16777216 * 64];
        for (var i = 0; i < 16777216; i++)
        {
            Buffer.BlockCopy(bytesToRepeat.ToArray(), 0, input, i * 64, 64);
        }
        var expected =
            Convert.FromHexString(
                "b47c933421ea2db1" +
                "49ad6e10fce6c7f9" +
                "3d0752380180ffd7" +
                "f4629a712134831d" +
                "77be6091b819ed35" +
                "2c2967a2e2d4fa50" +
                "50723c9630691f1a" +
                "05a7281dbe6c1086"
            );

        var actual = sha2.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase4()
    {
        var sha2 = new Sha2();
        var input = new byte[1000000];
        Array.Fill<byte>(input, 0x61);
        var expected =
            Convert.FromHexString(
                "e718483d0ce76964" +
                "4e2e42c7bc15b463" +
                "8e1f98b13b204428" +
                "5632a803afa973eb" +
                "de0ff244877ea60a" +
                "4cb0432ce577c31b" +
                "eb009c5c2c49aa2e" +
                "4eadb217ad8cc09b"
                );
        var actual = sha2.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }
}