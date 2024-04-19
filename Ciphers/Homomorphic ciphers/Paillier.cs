using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Math;
using static Homomorphic_ciphers.Paillier;

namespace Homomorphic_ciphers;

public class Paillier(PaillierPublicKey publicKey, PaillierPrivateKey privateKey)
{
    public static PaillierKeyPair GenerateKeys()
    {
        var secureRandom = new SecureRandom();
        BigInteger p, q, n, pMinusOne, qMinusOne;

        do
        {
            p = BigInteger.ProbablePrime(256, secureRandom);
            pMinusOne = p.Subtract(BigInteger.One);
            q = BigInteger.ProbablePrime(256, secureRandom);
            qMinusOne = q.Subtract(BigInteger.One);
            n = p.Multiply(q);
        } while (!n.Gcd(pMinusOne).Gcd(qMinusOne).Equals(BigInteger.One));

        // Least common multiple
        // (a / gcf(a, b)) * b
        BigInteger lamda = pMinusOne.Divide(pMinusOne.Gcd(qMinusOne)).Multiply(qMinusOne),
            g, mu;
        do
        {
            g = BigIntegers.CreateRandomInRange(BigInteger.Two, n.Pow(2), secureRandom);
            mu = g.ModPow(lamda, n.Multiply(n)).Subtract(BigInteger.One).Divide(n).ModInverse(n);
        } while (mu.Equals(BigInteger.One));

        return new PaillierKeyPair(new PaillierPublicKey(n, g), new PaillierPrivateKey(lamda, mu));
    }

    public BigInteger Encrypt(BigInteger plainData)
    {
        if(plainData.CompareTo(BigInteger.Zero) < 0 || plainData.CompareTo(publicKey.N) >= 0)
            throw new ArgumentOutOfRangeException(nameof(plainData), "Plain data must be in the range [0, n)");

        BigInteger r;
        do
        {
            r = BigIntegers.CreateRandomInRange(BigInteger.Two, publicKey.N, new SecureRandom());
        } while (!r.Gcd(publicKey.N).Equals(BigInteger.One));

        var squareN = publicKey.N.Multiply(publicKey.N);
        return publicKey.G.ModPow(plainData, squareN).Multiply(r.ModPow(publicKey.N, squareN)).Mod(squareN);
    }

    public BigInteger Decrypt(BigInteger encryptedData)
    {
        return L(encryptedData.ModPow(privateKey.Lambda, publicKey.N.Multiply(publicKey.N))).Multiply(privateKey.Mu).Mod(publicKey.N);
    }

    private BigInteger L(BigInteger u)
    {
        return u.Subtract(BigInteger.One).Divide(publicKey.N);
    }

    public readonly struct PaillierPublicKey(BigInteger n, BigInteger g)
    {
        public BigInteger N { get; init; } = n;
        public BigInteger G { get; init; } = g;
    }

    public readonly struct PaillierPrivateKey(BigInteger lambda, BigInteger mu)
    {
        public BigInteger Lambda { get; init; } = lambda;
        public BigInteger Mu { get; init; } = mu;
    }

    public readonly struct PaillierKeyPair(PaillierPublicKey publicKey, PaillierPrivateKey privateKey)
    {
        public PaillierPublicKey Public { get; init; } = publicKey;
        public PaillierPrivateKey Private { get; init; } = privateKey;
    }
}