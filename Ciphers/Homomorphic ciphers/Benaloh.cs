using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Math;

namespace Homomorphic_ciphers;

public class Benaloh(Benaloh.BenalohPublicKey publicKey, Benaloh.BenalohPrivateKey privateKey)
{
    public static BenalohKeyPair GenerateKeys()
    {
        var secureRandom = new SecureRandom();
        BigInteger p = BigInteger.ProbablePrime(256, secureRandom),
            q = BigInteger.ProbablePrime(256, secureRandom),
            n = p.Multiply(q),
            pMinusOne = p.Subtract(BigInteger.One),
            qMinusOne = q.Subtract(BigInteger.One),
            phi = pMinusOne.Multiply(qMinusOne),
            pMinusOneDivided = BigInteger.Zero;

        // Calculate block size r
        BigInteger r = BigInteger.ProbablePrime(256, secureRandom),
            pMinusOneModR = pMinusOne.Mod(r),
            gcdRPminusOneDivided = r.Gcd(pMinusOneDivided),
            gcdRQminusOne = r.Gcd(qMinusOne);

        while (!(pMinusOneModR.Equals(BigInteger.Zero) &&
                 gcdRPminusOneDivided.Equals(BigInteger.One) &&
                 gcdRQminusOne
                     .Equals(BigInteger.One)))
        {
            r = BigInteger.ProbablePrime(256, secureRandom);
            pMinusOneDivided = pMinusOne.Divide(r);
            pMinusOneModR = pMinusOne.Mod(r);
            gcdRPminusOneDivided = r.Gcd(pMinusOneDivided);
            gcdRQminusOne = r.Gcd(qMinusOne);
        }
        
        BigInteger y,
            phiDivided = phi.Divide(r),
            nMinusOne = n.Subtract(BigInteger.One);
        do
        {
            y = BigIntegers.CreateRandomInRange(BigInteger.One, nMinusOne, secureRandom);
        } while(!y.Gcd(n).Equals(BigInteger.One) && y.ModPow(phiDivided, n).Equals(BigInteger.One));

        return new BenalohKeyPair(new BenalohPublicKey(y, r, n), new BenalohPrivateKey(p, q));
    }

    public BigInteger Encrypt(BigInteger plainData)
    {
        BigInteger u;
        do
        {
            u = BigIntegers.CreateRandomInRange(BigInteger.One, publicKey.N.Subtract(BigInteger.One),
                new SecureRandom());
        } while (!u.Gcd(publicKey.N).Equals(BigInteger.One));
        var c = publicKey.Y.ModPow(plainData, publicKey.N).ModMultiply(u.ModPow(publicKey.R, publicKey.N),publicKey.N);
        return c;
    }

    public BigInteger Decrypt(BigInteger encryptedData)
    {
        BigInteger phi = privateKey.P.Subtract(BigInteger.One).Multiply(privateKey.Q.Subtract(BigInteger.One)),
            phiDivided = phi.Divide(publicKey.R),
            a = encryptedData.Mod(publicKey.N).ModPow(phiDivided, publicKey.N),
            x = publicKey.Y.ModPow(phiDivided, publicKey.N),
            md = BigInteger.Zero;

        while (!x.ModPow(md, publicKey.N).Equals(a))
            md = md.Add(BigInteger.One);

        return md;
    }

    public readonly struct BenalohPublicKey(BigInteger y, BigInteger r, BigInteger n)
    {
        public BigInteger Y { get; init; } = y;
        public BigInteger R { get; init; } = r;
        public BigInteger N { get; init; } = n;
    }

    public readonly struct BenalohPrivateKey(BigInteger p, BigInteger q)
    {
        public BigInteger P { get; init; } = p;
        public BigInteger Q { get; init; } = q;
    }

    public readonly struct BenalohKeyPair(BenalohPublicKey publicKey, BenalohPrivateKey privateKey)
    {
        public BenalohPublicKey Public { get; init; } = publicKey;
        public BenalohPrivateKey Private { get; init; } = privateKey;
    }
}