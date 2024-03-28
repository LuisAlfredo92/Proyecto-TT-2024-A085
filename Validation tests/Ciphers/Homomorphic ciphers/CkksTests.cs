using Homomorphic_ciphers;
using Microsoft.Research.SEAL;

namespace Validation_tests.Ciphers.Homomorphic_ciphers;

public class CkksTests
{
    [Fact]
    public void TestCase1()
    {
        Ckks ckks = new();
        const long plainData = 5;

        var encryptedData = ckks.Encrypt(plainData);
        var decryptedData = ckks.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        var context = Ckks.GenerateContext();
        Ckks.GenerateKeys(context, out var publicKey, out var secretKey);
        Ckks ckks = new(context, publicKey, secretKey);
        var plainData = Random.Shared.NextInt64(2048);

        var encryptedData = ckks.Encrypt(plainData);
        long decryptedData = ckks.Decrypt(encryptedData),
            result = Math.Abs(decryptedData - plainData);

        Assert.True(result <= 5);
    }

    /// <summary>
    /// Test the homomorphic addition property of the CKKS cryptosystem
    /// </summary>
    [Fact]
    public void TestCase3()
    {
        var context = Ckks.GenerateContext();
        Ckks.GenerateKeys(context, out var publicKey, out var secretKey);
        Ckks ckks = new(context, publicKey, secretKey);
        long a = Random.Shared.NextInt64(1024),
            b = Random.Shared.NextInt64(1024),
            addition = a + b;

        Ciphertext encryptedA = ckks.Encrypt(a),
            encryptedB = ckks.Encrypt(b),
            encryptedAddition = new();
        var evaluator = ckks.Evaluator;
        evaluator.Add(encryptedA, encryptedB, encryptedAddition);

        long decryptedData = ckks.Decrypt(encryptedAddition),
            result = Math.Abs(addition - decryptedData);

        Assert.True(result <= 5);
    }

    /// <summary>
    /// Test the homomorphic subtraction property of the CKKS cryptosystem
    /// </summary>
    [Fact]
    public void TestCase4()
    {
        var context = Ckks.GenerateContext();
        Ckks.GenerateKeys(context, out var publicKey, out var secretKey);
        Ckks ckks = new(context, publicKey, secretKey);
        long a = Random.Shared.NextInt64(1024),
            b = Random.Shared.NextInt64(1024),
            subtraction = a - b;

        Ciphertext encryptedA = ckks.Encrypt(a),
            encryptedB = ckks.Encrypt(b),
            encryptedSubtraction = new();
        var evaluator = ckks.Evaluator;
        evaluator.Sub(encryptedA, encryptedB, encryptedSubtraction);

        long decryptedData = ckks.Decrypt(encryptedSubtraction),
            result = Math.Abs(subtraction - decryptedData);

        Assert.True(result <= 5);
    }

    /// <summary>
    /// Test the homomorphic multiplication property of the CKKS cryptosystem
    /// </summary>
    [Fact]
    public void TestCase5()
    {
        var context = Ckks.GenerateContext();
        Ckks.GenerateKeys(context, out var publicKey, out var secretKey);
        Ckks ckks = new(context, publicKey, secretKey);
        long a = Random.Shared.NextInt64(1024),
            b = Random.Shared.NextInt64(1024),
            multiplication = a * b;

        Ciphertext encryptedA = ckks.Encrypt(a),
            encryptedB = ckks.Encrypt(b),
            encryptedMultiplication = new();
        var evaluator = ckks.Evaluator;
        evaluator.Multiply(encryptedA, encryptedB, encryptedMultiplication);

        long decryptedData = ckks.Decrypt(encryptedMultiplication),
            result = Math.Abs(multiplication - decryptedData);

        Assert.True(result <= 5);
    }

    /// <summary>
    /// Test the homomorphic square power property of the CKKS cryptosystem
    /// </summary>
    [Fact]
    public void TestCase6()
    {
        var context = Ckks.GenerateContext();
        Ckks.GenerateKeys(context, out var publicKey, out var secretKey);
        Ckks ckks = new(context, publicKey, secretKey);
        long a = Random.Shared.NextInt64(1024),
            square = a * a;

        Ciphertext encryptedA = ckks.Encrypt(a),
            encryptedSquare = new();
        var evaluator = ckks.Evaluator;
        evaluator.Square(encryptedA, encryptedSquare);

        long decryptedData = ckks.Decrypt(encryptedSquare),
            result = Math.Abs(square - decryptedData);

        Assert.True(result <= 5);
    }
}