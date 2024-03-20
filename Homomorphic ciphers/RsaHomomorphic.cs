using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Homomorphic_ciphers;

public class RsaHomomorphic(RsaKeyParameters publicKey, RsaKeyParameters privateKey)
{

    public BigInteger Encrypt(BigInteger plainData)
    {
        return plainData.ModPow(publicKey.Exponent, publicKey.Modulus);
    }

    public BigInteger Decrypt(BigInteger encryptedData)
    {
        return encryptedData.ModPow(privateKey.Exponent, privateKey.Modulus);
    }
}