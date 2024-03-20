using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace Asymmetric_ciphers;

public class ElGamal
{
    private readonly ElGamalEngine _elGamalEngineEncrypt;
    private readonly ElGamalEngine _elGamalEngineDecrypt;

    public ElGamal(ElGamalPublicKeyParameters publicParameters, ElGamalPrivateKeyParameters privateParameters)
    {
        _elGamalEngineEncrypt = new ElGamalEngine();
        _elGamalEngineDecrypt = new ElGamalEngine();

        _elGamalEngineEncrypt.Init(true, publicParameters);
        _elGamalEngineDecrypt.Init(false, privateParameters);
    }

    public byte[] Encrypt(byte[] plainText)
    {
        return _elGamalEngineEncrypt.ProcessBlock(plainText, 0, plainText.Length);
    }

    public byte[] Decrypt(byte[] cipherData)
    {
        return _elGamalEngineDecrypt.ProcessBlock(cipherData, 0, cipherData.Length);
    }
}