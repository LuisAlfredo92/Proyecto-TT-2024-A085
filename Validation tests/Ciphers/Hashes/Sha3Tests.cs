using Hashes;

namespace Validation_tests.Ciphers.Hashes;

public class Sha3Tests
{
    /// <summary>
    /// Input message: "abc", the bit string (0x)616263 of length 24 bits. 
    /// </summary>
    [Fact]
    public void TestCase1()
    {
        var input = "abc"u8.ToArray();
        var expected =
            Convert.FromHexString(
                "b751850b1a57168a" +
                "5693cd924b6b096e" +
                "08f621827444f70d" +
                "884f5d0240d2712e" +
                "10e116e9192af3c9" +
                "1a7ec57647e39340" +
                "57340b4cf408d5a5" +
                "6592f8274eec53f0"
                );
        var actual = Sha3.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    /// <summary>
    /// Input message: the empty string "", a bit string of length 0. 
    /// </summary>
    [Fact]
    public void TestCase2()
    {
        var input = ""u8.ToArray();
        var expected =
            Convert.FromHexString(
                "a69f73cca23a9ac5" +
                "c8b567dc185a756e" +
                "97c982164fe25859" +
                "e0d1dcc1475c80a6" +
                "15b2123af1f5f94c" +
                "11e3e9402c3ac558" +
                "f500199d95b6d3e3" +
                "01758586281dcd26"
                );
        var actual = Sha3.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase3()
    {
        var input = "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq"u8.ToArray();
        var expected =
            Convert.FromHexString(
                "04a371e84ecfb5b8" +
                "b77cb48610fca818" +
                "2dd457ce6f326a0f" +
                "d3d7ec2f1e91636d" +
                "ee691fbe0c985302" +
                "ba1b0d8dc78c0863" +
                "46b533b49c030d99" +
                "a27daf1139d6e75e"
                );
        var actual = Sha3.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase4()
    {
        var input = new byte[1000000];
        Array.Fill<byte>(input, 0x61);
        var expected =
            Convert.FromHexString(
                "3c3a876da14034ab" +
                "60627c077bb98f7e" +
                "120a2a5370212dff" +
                "b3385a18d4f38859" +
                "ed311d0a9d5141ce" +
                "9cc5c66ee689b266" +
                "a8aa18ace8282a0e" +
                "0db596c90b0a7b87"
                );
        var actual = Sha3.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }

    [Fact]
    public void TestCase5()
    {
        Span<byte> bytesToRepeat = stackalloc byte[64];
        "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmno"u8.ToArray().CopyTo(bytesToRepeat);
        var input = new byte[16777216 * 64];
        for (var i = 0; i < 16777216; i++)
        {
            Buffer.BlockCopy(bytesToRepeat.ToArray(), 0, input, i * 64, 64);
        }
        var expected =
            Convert.FromHexString(
                "235ffd53504ef836" +
                "a1342b488f483b39" +
                "6eabbfe642cf78ee" +
                "0d31feec788b23d0" +
                "d18d5c339550dd59" +
                "58a500d4b95363da" +
                "1b5fa18affc1bab2" +
                "292dc63b7d85097c"
            );

        var actual = Sha3.Hash(input);
        Assert.Equal(expected, actual.ToArray());
    }
}