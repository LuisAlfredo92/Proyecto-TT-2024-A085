using System.Security.Cryptography;
using System.Text;
using Hashes;

namespace Validation_tests.Hashes;

/// <summary>
/// Tests for <see cref="Pbkdf2"/>.
/// Test vectos from <a href="https://github.com/brycx/Test-Vector-Generation/blob/master/PBKDF2/pbkdf2-hmac-sha2-test-vectors.md">https://github.com/brycx/Test-Vector-Generation/blob/master/PBKDF2/pbkdf2-hmac-sha2-test-vectors.md</a>.
/// </summary>
public class Pbkdf2Tests
{
    [Theory]
    [InlineData("password", "salt", 1, 20, "SHA256", "120fb6cffcf8b32c43e7225256c4f837a86548c9")]
    [InlineData("password", "salt", 1, 20, "SHA384", "c0e14f06e49e32d73f9f52ddf1d0c5c719160923")]
    [InlineData("password", "salt", 1, 20, "SHA512", "867f70cf1ade02cff3752599a3a53dc4af34c7a6")]
    public void TestCase1(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("password", "salt", 2, 20, "SHA256", "ae4d0c95af6b46d32d0adff928f06dd02a303f8e")]
    [InlineData("password", "salt", 2, 20, "SHA384", "54f775c6d790f21930459162fc535dbf04a93918")]
    [InlineData("password", "salt", 2, 20, "SHA512", "e1d9c16aa681708a45f5c7c4e215ceb66e011a2e")]
    public void TestCase2(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("password", "salt", 4096, 20, "SHA256", "c5e478d59288c841aa530db6845c4c8d962893a0")]
    [InlineData("password", "salt", 4096, 20, "SHA384", "559726be38db125bc85ed7895f6e3cf574c7a01c")]
    [InlineData("password", "salt", 4096, 20, "SHA512", "d197b1b33db0143e018b12f3d1d1479e6cdebdcc")]
    public void TestCase3(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("password", "salt", 16777216, 20, "SHA256", "cf81c66fe8cfc04d1f31ecb65dab4089f7f179e8")]
    [InlineData("password", "salt", 16777216, 20, "SHA384", "a7fdb349ba2bfa6bf647bb0161bae1320df27e64")]
    [InlineData("password", "salt", 16777216, 20, "SHA512", "6180a3ceabab45cc3964112c811e0131bca93a35")]
    public void TestCase4(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("passwordPASSWORDpassword", "saltSALTsaltSALTsaltSALTsaltSALTsalt", 4096, 25, "SHA256", "348c89dbcbd32b2f32d814b8116e84cf2b17347ebc1800181c")]
    [InlineData("passwordPASSWORDpassword", "saltSALTsaltSALTsaltSALTsaltSALTsalt", 4096, 25, "SHA384", "819143ad66df9a552559b9e131c52ae6c5c1b0eed18f4d283b")]
    [InlineData("passwordPASSWORDpassword", "saltSALTsaltSALTsaltSALTsaltSALTsalt", 4096, 25, "SHA512", "8c0511f4c6e597c6ac6315d8f0362e225f3c501495ba23b868")]
    public void TestCase5(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("pass\0word", "sa\0lt", 4096, 16, "SHA256", "89b69d0516f829893c696226650a8687")]
    [InlineData("pass\0word", "sa\0lt", 4096, 16, "SHA384", "a3f00ac8657e095f8e0823d232fc60b3")]
    [InlineData("pass\0word", "sa\0lt", 4096, 16, "SHA512", "9d9e9c4cd21fe4be24d5b8244c759665")]
    public void TestCase6(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("passwd", "salt", 1, 128, "SHA256", "55ac046e56e3089fec1691c22544b605f94185216dde0465e68b9d57c20dacbc49ca9cccf179b645991664b39d77ef317c71b845b1e30bd509112041d3a19783c294e850150390e1160c34d62e9665d659ae49d314510fc98274cc79681968104b8f89237e69b2d549111868658be62f59bd715cac44a1147ed5317c9bae6b2a")]
    [InlineData("passwd", "salt", 1, 128, "SHA384", "cd3443723a41cf1460cca9efeede428a8898a82d2ad4d1fc5cca08ed3f4d3cb47a62a70b3cb9ce65dcbfb9fb9d425027a8be69b53e2a22674b0939e5e0a682f76d21f449ad184562a3bc4c519b4d048de6d8e0999fb88770f95e40185e19fc8b68767417ccc064f47a455d045b3bafda7e81b97ad0e4c5581af1aa27871cd5e4")]
    [InlineData("passwd", "salt", 1, 128, "SHA512", "c74319d99499fc3e9013acff597c23c5baf0a0bec5634c46b8352b793e324723d55caa76b2b25c43402dcfdc06cdcf66f95b7d0429420b39520006749c51a04ef3eb99e576617395a178ba33214793e48045132928a9e9bf2661769fdc668f31798597aaf6da70dd996a81019726084d70f152baed8aafe2227c07636c6ddece")]
    public void TestCase7(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("Password", "NaCl", 80000, 128, "SHA256", "4ddcd8f60b98be21830cee5ef22701f9641a4418d04c0414aeff08876b34ab56a1d425a1225833549adb841b51c9b3176a272bdebba1d078478f62b397f33c8d62aae85a11cdde829d89cb6ffd1ab0e63a981f8747d2f2f9fe5874165c83c168d2eed1d2d5ca4052dec2be5715623da019b8c0ec87dc36aa751c38f9893d15c3")]
    [InlineData("Password", "NaCl", 80000, 128, "SHA384", "11c198987730fa113458053cd5cc9b51d7024a35f9134f1ee8740923c901aab23bbaea43686981b6e6a9f4130a1401daeeec74060246ebac958f3cfc3c65579b6e3d08b94ade5fc257a6902a0a1664b8dbd5a8ae2af70438931d3f3679abffc7a17770582f1ee413cc0d9914ce5f8143c8a7dc9c43fbc31e3d41b2030fb73c02")]
    [InlineData("Password", "NaCl", 80000, 128, "SHA512", "e6337d6fbeb645c794d4a9b5b75b7b30dac9ac50376a91df1f4460f6060d5addb2c1fd1f84409abacc67de7eb4056e6bb06c2d82c3ef4ccd1bded0f675ed97c65c33d39f81248454327aa6d03fd049fc5cbb2b5e6dac08e8ace996cdc960b1bd4530b7e754773d75f67a733fdb99baf6470e42ffcb753c15c352d4800fb6f9d6")]
    public void TestCase8(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("Password", "sa\0lt", 4096, 256, "SHA256", "436c82c6af9010bb0fdb274791934ac7dee21745dd11fb57bb90112ab187c495ad82df776ad7cefb606f34fedca59baa5922a57f3e91bc0e11960da7ec87ed0471b456a0808b60dff757b7d313d4068bf8d337a99caede24f3248f87d1bf16892b70b076a07dd163a8a09db788ae34300ff2f2d0a92c9e678186183622a636f4cbce15680dfea46f6d224e51c299d4946aa2471133a649288eef3e4227b609cf203dba65e9fa69e63d35b6ff435ff51664cbd6773d72ebc341d239f0084b004388d6afa504eee6719a7ae1bb9daf6b7628d851fab335f1d13948e8ee6f7ab033a32df447f8d0950809a70066605d6960847ed436fa52cdfbcf261b44d2a87061")]
    [InlineData("Password", "sa\0lt", 4096, 256, "SHA384", "cf6f194aaf4e970afea1f41169045029e34759e124a670b5f73053da552a190ad2d7085533b8b22901f0e3caeeb431ba673468f981352dfcbe517699db791777cf52346a460b093c59ea300fb18daee270e2ea8473806da1663cebe7438b51fe56ba832c13d88ad5b2e46404457c34cc6ad8e5cd8707a1acfa737f3617628a5983d8d10fa16a92652cfa736d4610132710a517c216cc3252e6c2b8aae0275d04a49756fa5bf1bb067bc367d1b8c80c3df7dc22ee74b4be4150871624bfdde3f86f5fbd4e0828af7d5a4f01b5605e54471435d827eaecf199db315ae60d1a6350105c0e1a71b40518a4a66ebba4792a511f8f52aeac961ebea215f8fb89ba998b")]
    [InlineData("Password", "sa\0lt", 4096, 256, "SHA512", "10176fb32cb98cd7bb31e2bb5c8f6e425c103333a2e496058e3fd2bd88f657485c89ef92daa0668316bc23ebd1ef88f6dd14157b2320b5d54b5f26377c5dc279b1dcdec044bd6f91b166917c80e1e99ef861b1d2c7bce1b961178125fb86867f6db489a2eae0022e7bc9cf421f044319fac765d70cb89b45c214590e2ffb2c2b565ab3b9d07571fde0027b1dc57f8fd25afa842c1056dd459af4074d7510a0c020b914a5e202445d4d3f151070589dd6a2554fc506018c4f001df6239643dc86771286ae4910769d8385531bba57544d63c3640b90c98f1445ebdd129475e02086b600f0beb5b05cc6ca9b3633b452b7dad634e9336f56ec4c3ac0b4fe54ced8")]
    public void TestCase9(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    [Theory]
    [InlineData("password", "salt", 1, 20, "SHA1", "0c60c80f961f0e71f3a9b524af6012062fe037a6")]
    [InlineData("password", "salt", 2, 20, "SHA1", "ea6c014dc72d6f8ccd1ed92ace1d41f0d8de8957")]
    [InlineData("password", "salt", 4096, 20, "SHA1", "4b007901b765489abead49d926f721d065a429c1")]
    [InlineData("password", "salt", 16777216, 20, "SHA1", "eefe3d61cd4da4e4e9945b3d6ba2158c2634e984")]
    [InlineData("passwordPASSWORDpassword", "saltSALTsaltSALTsaltSALTsaltSALTsalt", 4096, 25, "SHA1", "3d2eec4fe41c849b80c8d83662c0e44a8b291a964cf2f07038")]
    [InlineData("pass\0word", "sa\0lt", 4096, 16, "SHA1", "56fa6aa75548099dcc37d7f03425e0c3")]
    public void TestCase10(string p, string s, int c, int dkLen, string name, string expected)
    {
        var pbkdf2 = new Pbkdf2(Encoding.UTF8.GetBytes(s), c, dkLen, GetHashAlgorithm(name));
        var actual = pbkdf2.Hash(Encoding.UTF8.GetBytes(p));
        Assert.Equal(Convert.FromHexString(expected), actual);
    }

    private static HashAlgorithmName GetHashAlgorithm(string name) =>
        name switch
        {
            "MD5" => HashAlgorithmName.MD5,
            "SHA1" => HashAlgorithmName.SHA1,
            "SHA256" => HashAlgorithmName.SHA256,
            "SHA384" => HashAlgorithmName.SHA384,
            "SHA512" => HashAlgorithmName.SHA512,
            _ => throw new NotSupportedException()
        };
}