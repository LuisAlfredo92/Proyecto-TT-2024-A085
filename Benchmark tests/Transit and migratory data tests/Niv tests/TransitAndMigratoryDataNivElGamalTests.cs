﻿using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Transit_and_migratory_data.Niv;

namespace Niv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataNivElGamalTests
{
    private ElGamal _elGamal = null!;
    private byte[] _niv = null!;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptNivElGamal))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _niv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
    }

    [Benchmark]
    public byte[] EncryptNivElGamal() => _elGamal.Encrypt(_niv);

    [GlobalSetup(Target = nameof(DecryptNivElGamal))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedNiv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
        _niv = _elGamal.Encrypt(generatedNiv);
    }

    [Benchmark]
    public byte[] DecryptNivElGamal() => _elGamal.Decrypt(_niv);
}