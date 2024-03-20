using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Fpe;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Crypto;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FPE_ciphers;

public class TwoFishFpe
{
    private readonly FpeFf3_1Engine _algorithmEncrypt;
    private readonly FpeFf3_1Engine _algorithmDecrypt;
    private readonly BasicAlphabetMapper _alphabetMapper;

    public TwoFishFpe(Span<byte> key, char[] alphabet, string tweakString = "0123456")
    {
        _alphabetMapper = new BasicAlphabetMapper(alphabet);
        var tweak = Encoding.ASCII.GetBytes(tweakString);

        FpeParameters fpeKeyParam = new(new KeyParameter(key), _alphabetMapper.Radix, tweak);
        IBlockCipher cipherEncrypt = new TwofishEngine(),
            cipherDecrypt = new TwofishEngine();

        _algorithmEncrypt = new FpeFf3_1Engine(cipherEncrypt);
        _algorithmDecrypt = new FpeFf3_1Engine(cipherDecrypt);
        _algorithmEncrypt.Init(true, fpeKeyParam);
        _algorithmDecrypt.Init(false, fpeKeyParam);
    }

    public char[] Encrypt([Length(4, 32)] char[] plainText)
    {
        byte[] cipherText = new byte[plainText.Length],
            convertedPlainTextData = _alphabetMapper.ConvertToIndexes(plainText);

        _algorithmEncrypt.ProcessBlock(convertedPlainTextData, 0, convertedPlainTextData.Length, cipherText, 0);

        return _alphabetMapper.ConvertToChars(cipherText);
    }

    public char[] Decrypt([Length(4, 32)] char[] cipherText)
    {
        byte[] plainText = new byte[cipherText.Length],
            convertedCipherText = _alphabetMapper.ConvertToIndexes(cipherText);

        _algorithmDecrypt.ProcessBlock(convertedCipherText, 0, convertedCipherText.Length, plainText, 0);

        return _alphabetMapper.ConvertToChars(plainText);
    }
}