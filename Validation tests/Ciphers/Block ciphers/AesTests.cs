using System.Security.Cryptography;
using Identifying_data.Phone_numbers;
using Xunit.Abstractions;
using Aes = BlockCiphers.Aes;

namespace Validation_tests.Ciphers.Block_ciphers;

/// <summary>
///     Tests for the AES encryption and decryption.
///     Most of the tests were taken from the NIST test vectors:
///     https://csrc.nist.gov/CSRC/media/Projects/Cryptographic-Standards-and-Guidelines/documents/examples/AES_GCM.pdf
/// </summary>
public class AesTests(ITestOutputHelper testOutputHelper)
{
    /// <summary>
    ///     Tests the default AES encryption and decryption
    /// </summary>
    [Fact]
    public void TestAes()
    {
        Span<byte> key = stackalloc byte[16];
        byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize],
            plainBytes = new byte[16];
        Random.Shared.NextBytes(plainBytes);
        Random.Shared.NextBytes(nonce);
        Random.Shared.NextBytes(key);

        Aes aes = new(key, nonce);

        var encrypted = aes.Encrypt(plainBytes, out var tag);
        var decrypted = aes.Decrypt(encrypted, tag);

        Assert.Equal(plainBytes, decrypted);
    }

    /// <summary>
    ///     Tests the AES encryption and decryption with a known key and nonce (test case 1)
    /// </summary>
    [Fact]
    public void AesTestCase1()
    {
        Span<byte> key =
            [0xFE, 0xFF, 0xE9, 0x92, 0x86, 0x65, 0x73, 0x1C, 0x6D, 0x6A, 0x8F, 0x94, 0x67, 0x30, 0x83, 0x08];
        byte[] plainBytes = [],
            encryptedExpected = [],
            nonce = [0xCA, 0xFE, 0xBA, 0xBE, 0xFA, 0xCE, 0xDB, 0xAD, 0xDE, 0xCA, 0xF8, 0x88],
            tagExpected =
                [0x32, 0x47, 0x18, 0x4B, 0x3C, 0x4F, 0x69, 0xA4, 0x4D, 0xBC, 0xD2, 0x28, 0x87, 0xBB, 0xB4, 0x18];

        Aes testAes = new(key, nonce);

        var encrypted = testAes.Encrypt(plainBytes, out var tag);
        var decrypted = testAes.Decrypt(encrypted, tag);

        Assert.Equal(encryptedExpected, encrypted);
        Assert.Equal(tagExpected, tag);
        Assert.Equal(plainBytes, decrypted);
    }

    /// <summary>
    ///     Tests the AES encryption and decryption with a known key and nonce (test case 2)
    /// </summary>
    [Fact]
    public void AesTestCase2()
    {
        Span<byte> key =
            [0xFE, 0xFF, 0xE9, 0x92, 0x86, 0x65, 0x73, 0x1C, 0x6D, 0x6A, 0x8F, 0x94, 0x67, 0x30, 0x83, 0x08];
        byte[] plainBytes =
            [
                0xD9, 0x31, 0x32, 0x25, 0xF8, 0x84, 0x06, 0xE5, 0xA5, 0x59, 0x09, 0xC5, 0xAF, 0xF5, 0x26, 0x9A,
                0x86, 0xA7, 0xA9, 0x53, 0x15, 0x34, 0xF7, 0xDA, 0x2E, 0x4C, 0x30, 0x3D, 0x8A, 0x31, 0x8A, 0x72,
                0x1C, 0x3C, 0x0C, 0x95, 0x95, 0x68, 0x09, 0x53, 0x2F, 0xCF, 0x0E, 0x24, 0x49, 0xA6, 0xB5, 0x25,
                0xB1, 0x6A, 0xED, 0xF5, 0xAA, 0x0D, 0xE6, 0x57, 0xBA, 0x63, 0x7B, 0x39, 0x1A, 0xAF, 0xD2, 0x55
            ],
            encryptedExpected =
            [
                0x42, 0x83, 0x1E, 0xC2, 0x21, 0x77, 0x74, 0x24, 0x4B, 0x72, 0x21, 0xB7, 0x84, 0xD0, 0xD4, 0x9C,
                0xE3, 0xAA, 0x21, 0x2F, 0x2C, 0x02, 0xA4, 0xE0, 0x35, 0xC1, 0x7E, 0x23, 0x29, 0xAC, 0xA1, 0x2E,
                0x21, 0xD5, 0x14, 0xB2, 0x54, 0x66, 0x93, 0x1C, 0x7D, 0x8F, 0x6A, 0x5A, 0xAC, 0x84, 0xAA, 0x05,
                0x1B, 0xA3, 0x0B, 0x39, 0x6A, 0x0A, 0xAC, 0x97, 0x3D, 0x58, 0xE0, 0x91, 0x47, 0x3F, 0x59, 0x85
            ],
            nonce = [0xCA, 0xFE, 0xBA, 0xBE, 0xFA, 0xCE, 0xDB, 0xAD, 0xDE, 0xCA, 0xF8, 0x88],
            tagExpected =
                [0x4D, 0x5C, 0x2A, 0xF3, 0x27, 0xCD, 0x64, 0xA6, 0x2C, 0xF3, 0x5A, 0xBD, 0x2B, 0xA6, 0xFA, 0xB4];

        Aes testAes = new(key, nonce);

        var encrypted = testAes.Encrypt(plainBytes, out var tag);
        var decrypted = testAes.Decrypt(encrypted, tag);

        Assert.Equal(encryptedExpected, encrypted);
        Assert.Equal(tagExpected, tag);
        Assert.Equal(plainBytes, decrypted);
    }

    /// <summary>
    ///     Tests the AES encryption and decryption with a known key and nonce (test case 3)
    /// </summary>
    [Fact]
    public void AesTestCase3()
    {
        Span<byte> key =
            [0xFE, 0xFF, 0xE9, 0x92, 0x86, 0x65, 0x73, 0x1C, 0x6D, 0x6A, 0x8F, 0x94, 0x67, 0x30, 0x83, 0x08];
        byte[] plainBytes = [],
            encryptedExpected = [],
            nonce = [0xCA, 0xFE, 0xBA, 0xBE, 0xFA, 0xCE, 0xDB, 0xAD, 0xDE, 0xCA, 0xF8, 0x88],
            authenticationData =
            [
                0x3A, 0xD7, 0x7B, 0xB4, 0x0D, 0x7A, 0x36, 0x60, 0xA8, 0x9E, 0xCA, 0xF3, 0x24, 0x66, 0xEF, 0x97,
                0xF5, 0xD3, 0xD5, 0x85, 0x03, 0xB9, 0x69, 0x9D, 0xE7, 0x85, 0x89, 0x5A, 0x96, 0xFD, 0xBA, 0xAF,
                0x43, 0xB1, 0xCD, 0x7F, 0x59, 0x8E, 0xCE, 0x23, 0x88, 0x1B, 0x00, 0xE3, 0xED, 0x03, 0x06, 0x88,
                0x7B, 0x0C, 0x78, 0x5E, 0x27, 0xE8, 0xAD, 0x3F, 0x82, 0x23, 0x20, 0x71, 0x04, 0x72, 0x5D, 0xD4
            ],
            tagExpected =
                [0x5F, 0x91, 0xD7, 0x71, 0x23, 0xEF, 0x5E, 0xB9, 0x99, 0x79, 0x13, 0x84, 0x9B, 0x8D, 0xC1, 0xE9];

        Aes testAes = new(key, nonce, authenticationData);

        var encrypted = testAes.Encrypt(plainBytes, out var tag);
        var decrypted = testAes.Decrypt(encrypted, tag);

        Assert.Equal(encryptedExpected, encrypted);
        Assert.Equal(tagExpected, tag);
        Assert.Equal(plainBytes, decrypted);
    }

    /// <summary>
    ///     Tests the AES encryption and decryption with a known key and nonce (test case 4)
    /// </summary>
    [Fact]
    public void AesTestCase4()
    {
        Span<byte> key =
            [0xFE, 0xFF, 0xE9, 0x92, 0x86, 0x65, 0x73, 0x1C, 0x6D, 0x6A, 0x8F, 0x94, 0x67, 0x30, 0x83, 0x08];
        byte[] plainBytes =
            [
                0xD9, 0x31, 0x32, 0x25, 0xF8, 0x84, 0x06, 0xE5, 0xA5, 0x59, 0x09, 0xC5, 0xAF, 0xF5, 0x26, 0x9A,
                0x86, 0xA7, 0xA9, 0x53, 0x15, 0x34, 0xF7, 0xDA, 0x2E, 0x4C, 0x30, 0x3D, 0x8A, 0x31, 0x8A, 0x72,
                0x1C, 0x3C, 0x0C, 0x95, 0x95, 0x68, 0x09, 0x53, 0x2F, 0xCF, 0x0E, 0x24, 0x49, 0xA6, 0xB5, 0x25,
                0xB1, 0x6A, 0xED, 0xF5, 0xAA, 0x0D, 0xE6, 0x57, 0xBA, 0x63, 0x7B, 0x39, 0x1A, 0xAF, 0xD2, 0x55
            ],
            encryptedExpected =
            [
                0x42, 0x83, 0x1E, 0xC2, 0x21, 0x77, 0x74, 0x24, 0x4B, 0x72, 0x21, 0xB7, 0x84, 0xD0, 0xD4, 0x9C,
                0xE3, 0xAA, 0x21, 0x2F, 0x2C, 0x02, 0xA4, 0xE0, 0x35, 0xC1, 0x7E, 0x23, 0x29, 0xAC, 0xA1, 0x2E,
                0x21, 0xD5, 0x14, 0xB2, 0x54, 0x66, 0x93, 0x1C, 0x7D, 0x8F, 0x6A, 0x5A, 0xAC, 0x84, 0xAA, 0x05,
                0x1B, 0xA3, 0x0B, 0x39, 0x6A, 0x0A, 0xAC, 0x97, 0x3D, 0x58, 0xE0, 0x91, 0x47, 0x3F, 0x59, 0x85
            ],
            nonce = [0xCA, 0xFE, 0xBA, 0xBE, 0xFA, 0xCE, 0xDB, 0xAD, 0xDE, 0xCA, 0xF8, 0x88],
            authenticationData =
            [
                0x3A, 0xD7, 0x7B, 0xB4, 0x0D, 0x7A, 0x36, 0x60, 0xA8, 0x9E, 0xCA, 0xF3, 0x24, 0x66, 0xEF, 0x97,
                0xF5, 0xD3, 0xD5, 0x85, 0x03, 0xB9, 0x69, 0x9D, 0xE7, 0x85, 0x89, 0x5A, 0x96, 0xFD, 0xBA, 0xAF,
                0x43, 0xB1, 0xCD, 0x7F, 0x59, 0x8E, 0xCE, 0x23, 0x88, 0x1B, 0x00, 0xE3, 0xED, 0x03, 0x06, 0x88,
                0x7B, 0x0C, 0x78, 0x5E, 0x27, 0xE8, 0xAD, 0x3F, 0x82, 0x23, 0x20, 0x71, 0x04, 0x72, 0x5D, 0xD4
            ],
            tagExpected =
                [0x64, 0xC0, 0x23, 0x29, 0x04, 0xAF, 0x39, 0x8A, 0x5B, 0x67, 0xC1, 0x0B, 0x53, 0xA5, 0x02, 0x4D];

        Aes testAes = new(key, nonce, authenticationData);

        var encrypted = testAes.Encrypt(plainBytes, out var tag);
        var decrypted = testAes.Decrypt(encrypted, tag);

        Assert.Equal(encryptedExpected, encrypted);
        Assert.Equal(tagExpected, tag);
        Assert.Equal(plainBytes, decrypted);
    }

    /// <summary>
    ///     Tests the AES encryption and decryption with a known key and nonce (test case 5)
    /// </summary>
    [Fact]
    public void AesTestCase5()
    {
        Span<byte> key =
            [0xFE, 0xFF, 0xE9, 0x92, 0x86, 0x65, 0x73, 0x1C, 0x6D, 0x6A, 0x8F, 0x94, 0x67, 0x30, 0x83, 0x08];
        byte[] plainBytes =
            [
                0xD9, 0x31, 0x32, 0x25, 0xF8, 0x84, 0x06, 0xE5, 0xA5, 0x59, 0x09, 0xC5, 0xAF, 0xF5, 0x26, 0x9A,
                0x86, 0xA7, 0xA9, 0x53, 0x15, 0x34, 0xF7, 0xDA, 0x2E, 0x4C, 0x30, 0x3D, 0x8A, 0x31, 0x8A, 0x72,
                0x1C, 0x3C, 0x0C, 0x95, 0x95, 0x68, 0x09, 0x53, 0x2F, 0xCF, 0x0E, 0x24, 0x49, 0xA6, 0xB5, 0x25,
                0xB1, 0x6A, 0xED, 0xF5, 0xAA, 0x0D, 0xE6, 0x57, 0xBA, 0x63, 0x7B, 0x39
            ],
            encryptedExpected =
            [
                0x42, 0x83, 0x1E, 0xC2, 0x21, 0x77, 0x74, 0x24, 0x4B, 0x72, 0x21, 0xB7, 0x84, 0xD0, 0xD4, 0x9C,
                0xE3, 0xAA, 0x21, 0x2F, 0x2C, 0x02, 0xA4, 0xE0, 0x35, 0xC1, 0x7E, 0x23, 0x29, 0xAC, 0xA1, 0x2E,
                0x21, 0xD5, 0x14, 0xB2, 0x54, 0x66, 0x93, 0x1C, 0x7D, 0x8F, 0x6A, 0x5A, 0xAC, 0x84, 0xAA, 0x05,
                0x1B, 0xA3, 0x0B, 0x39, 0x6A, 0x0A, 0xAC, 0x97, 0x3D, 0x58, 0xE0, 0x91
            ],
            nonce = [0xCA, 0xFE, 0xBA, 0xBE, 0xFA, 0xCE, 0xDB, 0xAD, 0xDE, 0xCA, 0xF8, 0x88],
            authenticationData =
            [
                0x3A, 0xD7, 0x7B, 0xB4, 0x0D, 0x7A, 0x36, 0x60, 0xA8, 0x9E, 0xCA, 0xF3, 0x24, 0x66, 0xEF, 0x97,
                0xF5, 0xD3, 0xD5, 0x85
            ],
            tagExpected =
                [0xF0, 0x7C, 0x25, 0x28, 0xEE, 0xA2, 0xFC, 0xA1, 0x21, 0x1F, 0x90, 0x5E, 0x1B, 0x6A, 0x88, 0x1B];

        Aes testAes = new(key, nonce, authenticationData);

        var encrypted = testAes.Encrypt(plainBytes, out var tag);
        var decrypted = testAes.Decrypt(encrypted, tag);

        Assert.Equal(encryptedExpected, encrypted);
        Assert.Equal(tagExpected, tag);
        Assert.Equal(plainBytes, decrypted);
    }

    [Fact]
    public void ShortInputs()
    {
        Span<byte> key = stackalloc byte[16];
        byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize],
            plainBytes = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        Random.Shared.NextBytes(nonce);
        Random.Shared.NextBytes(key);

        Aes aes = new(key, nonce);

        var encrypted = aes.Encrypt(plainBytes, out _);

        testOutputHelper.WriteLine(plainBytes.Length.ToString());
        testOutputHelper.WriteLine(encrypted.Length.ToString());
    }
}