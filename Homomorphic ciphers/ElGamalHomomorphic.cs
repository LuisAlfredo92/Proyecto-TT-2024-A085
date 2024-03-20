using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Homomorphic_ciphers;

using Org.BouncyCastle.Crypto.Parameters;

public class ElGamalHomomorphic(ElGamalPublicKeyParameters publicKey, ElGamalPrivateKeyParameters privateKey)
{
    private readonly SecureRandom _random = new();

    public (BigInteger yotta, BigInteger delta) Encrypt(BigInteger plainData)
    {
        BigInteger k = BigIntegers.CreateRandomInRange(BigInteger.One,
                publicKey.Parameters.P.Subtract(BigInteger.Two),
                _random),
            yotta = publicKey.Parameters.G.ModPow(k, publicKey.Parameters.P),
            delta = publicKey.Y.ModPow(k, publicKey.Parameters.P).ModMultiply(plainData, publicKey.Parameters.P);
        return (yotta, delta);
    }
    
    public BigInteger Decrypt((BigInteger yotta, BigInteger delta) encryptedData)
    {
        BigInteger calculated = encryptedData.yotta.ModPow(privateKey.X, publicKey.Parameters.P),
            calculatedInverse = calculated.ModInverse(publicKey.Parameters.P);
        return encryptedData.delta.ModMultiply(calculatedInverse, publicKey.Parameters.P);
    }
}