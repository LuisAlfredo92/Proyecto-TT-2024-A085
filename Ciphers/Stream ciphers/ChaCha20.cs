using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Stream_ciphers;

public class ChaCha20
{
    private readonly ChaCha20Poly1305 _chaCha20Encrypt;
    private readonly ChaCha20Poly1305 _chaCha20Decrypt;

    public ChaCha20(ReadOnlySpan<byte> key, byte[] nonce, byte[]? associatedData = null)
    {
        _chaCha20Encrypt = new ChaCha20Poly1305();
        _chaCha20Decrypt = new ChaCha20Poly1305();
        var associatedData1 = associatedData ?? [];
        var cipherParameters = new AeadParameters(new KeyParameter(key), 128, nonce, associatedData1);
        _chaCha20Encrypt.Init(true, cipherParameters);
        _chaCha20Decrypt.Init(false, cipherParameters);
    }

    public byte[] Encrypt(byte[] plainText)
    {
        var cipherTextData = new byte[_chaCha20Encrypt.GetOutputSize(plainText.Length)];
        var processLength = _chaCha20Encrypt.ProcessBytes(plainText, 0, plainText.Length, cipherTextData, 0);
        _chaCha20Encrypt.DoFinal(cipherTextData, processLength);

        return cipherTextData;
    }

    public byte[] Decrypt(byte[] cipherData)
    {
        var plainTextData = new byte[_chaCha20Decrypt.GetOutputSize(cipherData.Length)];
        var processLength = _chaCha20Decrypt.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0);
        _chaCha20Decrypt.DoFinal(plainTextData, processLength);

        return plainTextData;
    }
}