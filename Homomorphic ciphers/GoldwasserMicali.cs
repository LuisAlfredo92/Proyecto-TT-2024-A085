using System.Collections;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Homomorphic_ciphers;

public class GoldwasserMicali(GoldwasserMicali.GmPublicKey publicKey, GoldwasserMicali.GmPrivateKey privateKey)
{

    public static GmKeyPair GenerateKeys()
    {
        var secureRandom = new SecureRandom();
        BigInteger p, q, a;
        do
        {
            p = BigInteger.ProbablePrime(256, secureRandom);
            q = BigInteger.ProbablePrime(256, secureRandom);
        } while (!p.Mod(BigInteger.Four).Equals(BigInteger.Three) || !q.Mod(BigInteger.Four).Equals(BigInteger.Three));

        BigInteger n = p.Multiply(q),
            pMinusOne = p.Subtract(BigInteger.One),
            qMinusOne = q.Subtract(BigInteger.One);
        bool xFound;

        do
        {
            a = new BigInteger(256, secureRandom);
            xFound = a.ModPow(pMinusOne.Divide(BigInteger.Two), p).Equals(pMinusOne) &&
                      a.ModPow(qMinusOne.Divide(BigInteger.Two), q).Equals(qMinusOne);
        } while (!xFound);

        return new GmKeyPair(new GmPublicKey(n, a), new GmPrivateKey(p, q));
    }

    public BigInteger[] Encrypt(byte[] plainData)
    {
        BitArray bitArray = new(plainData);
        return Encrypt(bitArray);
    }

    public BigInteger[] Encrypt(BitArray plainData)
    {
        SecureRandom secureRandom = new();
        var encryptedData = new BigInteger[plainData.Length];
        Parallel.For(0, plainData.Length, (i, _) =>
        {
            BigInteger b;
            do
            {
                b = BigIntegers.CreateRandomInRange(BigInteger.Two, publicKey.N.Subtract(BigInteger.One), secureRandom);
            } while (!b.Gcd(publicKey.N).Equals(BigInteger.One));

            encryptedData[i] = b.ModPow(BigInteger.Two, publicKey.N);
            if (plainData[i])
                encryptedData[i] = encryptedData[i].ModMultiply(publicKey.A, publicKey.N);
        });

        return encryptedData;
    }

    public BitArray Decrypt(BigInteger[] encryptedData)
    {
        BitArray bitArray = new(encryptedData.Length);
        BigInteger pMinusOne = privateKey.P.Subtract(BigInteger.One),
            qMinusOne = privateKey.Q.Subtract(BigInteger.One);
        Parallel.For(0, encryptedData.Length, (i, _) =>
        {
            var aux = encryptedData[i].ModPow(pMinusOne.Divide(BigInteger.Two), privateKey.P);

            // If aux is 1, them we check if it's a quadratic residue of q too
            if(aux.Equals(BigInteger.One))
                aux = encryptedData[i].ModPow(qMinusOne.Divide(BigInteger.Two), privateKey.Q);
            
            if(aux.Equals(BigInteger.One))
                bitArray[i] = false;
            else
                bitArray[i] = true;
        });

        return bitArray;
    }

    public readonly struct GmPublicKey(BigInteger n, BigInteger a)
    {
        public BigInteger N { get; init; } = n;
        public BigInteger A { get; init; } = a;
    }

    public readonly struct GmPrivateKey(BigInteger p, BigInteger q)
    {
        public BigInteger P { get; init; } = p;
        public BigInteger Q { get; init; } = q;
    }

    public readonly struct GmKeyPair(GmPublicKey publicKey, GmPrivateKey privateKey)
    {
        public GmPublicKey Public { get; init; } = publicKey;
        public GmPrivateKey Private { get; init; } = privateKey;
    }
}
