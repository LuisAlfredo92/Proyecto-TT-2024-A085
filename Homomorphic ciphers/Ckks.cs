using Microsoft.Research.SEAL;

namespace Homomorphic_ciphers;

public class Ckks
{
    private readonly Encryptor _encryptor;
    private readonly Decryptor _decryptor;
    public Evaluator Evaluator { get; }
    private readonly CKKSEncoder _encoder;
    private readonly double _scale;

    public Ckks()
    {
        var context = GenerateContext();
        GenerateKeys(context, out var publicKey, out var secretKey);
        _scale = Math.Pow(2.0, 40);
        _encryptor = new Encryptor(context, publicKey);
        _decryptor = new Decryptor(context, secretKey);
        Evaluator = new Evaluator(context);
        _encoder = new CKKSEncoder(context);
    }

    public Ckks(SEALContext context, PublicKey publicKey, SecretKey secretKey, double scale = 1099511627776)
    {
        _scale = scale;
        _encryptor = new Encryptor(context, publicKey);
        _decryptor = new Decryptor(context, secretKey);
        Evaluator = new Evaluator(context);
        _encoder = new CKKSEncoder(context);
    }

    public static SEALContext GenerateContext()
    {
        EncryptionParameters encryptionParameters = new(SchemeType.CKKS);
        const ulong polyModulusDegree = 16384;
        encryptionParameters.PolyModulusDegree = polyModulusDegree;
        encryptionParameters.CoeffModulus = CoeffModulus.Create(
            polyModulusDegree, [60, 40, 40, 60]);

        return new SEALContext(encryptionParameters);
    }

    public static void GenerateKeys(SEALContext context, out PublicKey publicKey, out SecretKey secretKey)
    {
        KeyGenerator keyGenerator = new(context);
        secretKey = keyGenerator.SecretKey;
        keyGenerator.CreatePublicKey(out publicKey);
    }

    public Ciphertext Encrypt(long plainData)
    {
        var plain = new Plaintext();
        _encoder.Encode(plainData, _scale, plain);
        var encrypted = new Ciphertext();
        _encryptor.Encrypt(plain, encrypted);
        return encrypted;
    }

    public long Decrypt(Ciphertext encryptedData)
    {
        var plain = new Plaintext();
        _decryptor.Decrypt(encryptedData, plain);
        ICollection<double> plainData = [];
        _encoder.Decode(plain, plainData);
        return (long) Math.Round(plainData.First());
    }
}