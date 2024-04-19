using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace BlockCiphers;

public class Blowfish
{
        private readonly PaddedBufferedBlockCipher _cbcCipher;
        private readonly PaddedBufferedBlockCipher _cbcDecipher;
    public Blowfish([Length(16, 32)] byte[] key, [Length(8, 8)] byte[] iv)
    {
        BlowfishEngine blowfishEncryptionEngine = new();
        BlowfishEngine blowfishDecryptionEngine = new();
        CbcBlockCipher symmetricBlockEncryptionMode = new(blowfishEncryptionEngine);
        CbcBlockCipher symmetricBlockDecryptionMode = new(blowfishDecryptionEngine);
        Pkcs7Padding padding = new();
        _cbcCipher = new PaddedBufferedBlockCipher(symmetricBlockEncryptionMode, padding);
        _cbcDecipher = new PaddedBufferedBlockCipher(symmetricBlockDecryptionMode, padding);

        _cbcCipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));
            _cbcDecipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));
    }

    public byte[] Encrypt(byte[] plainText)
    {
        var blockSize = _cbcCipher.GetBlockSize();
        var cipherTextData = new byte[_cbcCipher.GetOutputSize(plainText.Length)];

        int processLength = _cbcCipher.ProcessBytes(plainText, 0, plainText.Length, cipherTextData, 0),
            finalLength = _cbcCipher.DoFinal(cipherTextData, processLength);

        var finalCipherTextData = new byte[cipherTextData.Length - (blockSize - finalLength)];
        Array.Copy(cipherTextData, 0, finalCipherTextData, 0, finalCipherTextData.Length);

        return finalCipherTextData;
    }

    public byte[] Decrypt(byte[] cipherData)
    {
        var blockSize = _cbcDecipher.GetBlockSize();
        var plainTextData = new byte[_cbcDecipher.GetOutputSize(cipherData.Length)];

        int processLength = _cbcDecipher.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0),
            finalLength = _cbcDecipher.DoFinal(plainTextData, processLength);

        var finalPlainTextData = new byte[plainTextData.Length - (blockSize - finalLength)];
        Array.Copy(plainTextData, 0, finalPlainTextData, 0, finalPlainTextData.Length);
        return finalPlainTextData;
    }
}