﻿using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.ComponentModel.DataAnnotations;

namespace BlockCiphers;

public class Serpent
{
    private GcmBlockCipher _gcmCipher;
    private readonly GcmBlockCipher _gcmDecipher;
    private readonly SerpentEngine _serpentEngineEncrypt;
    private readonly AeadParameters _cipherParameters;

    public Serpent([Length(16, 32)] Span<byte> key, [Length(8, 8)] byte[] nonce, byte[]? associatedData = null)
    {
        _serpentEngineEncrypt = new();
        SerpentEngine serpentEngineDecrypt = new();
        _gcmCipher = new GcmBlockCipher(_serpentEngineEncrypt);
        _gcmDecipher = new GcmBlockCipher(serpentEngineDecrypt);
        var associatedData1 = associatedData ?? [];

        _cipherParameters = new AeadParameters(new KeyParameter(key), 128, nonce, associatedData1);
        _gcmCipher.Init(true, _cipherParameters);
        _gcmDecipher.Init(false, _cipherParameters);
    }

    public void Reset()
    {
        _gcmCipher = new GcmBlockCipher(_serpentEngineEncrypt);
        _gcmCipher.Init(true, _cipherParameters);
    }

    public byte[] Encrypt(byte[] plainText)
    {
        var cipherTextData = new byte[_gcmCipher.GetOutputSize(plainText.Length)];
        var processLength = _gcmCipher.ProcessBytes(plainText, 0, plainText.Length, cipherTextData, 0);
        _gcmCipher.DoFinal(cipherTextData, processLength);

        return cipherTextData;
    }

    public byte[] Decrypt(byte[] cipherData)
    {
        var plainTextData = new byte[_gcmDecipher.GetOutputSize(cipherData.Length)];
        var processLength = _gcmDecipher.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0);
        _gcmDecipher.DoFinal(plainTextData, processLength);

        return plainTextData;
    }
}