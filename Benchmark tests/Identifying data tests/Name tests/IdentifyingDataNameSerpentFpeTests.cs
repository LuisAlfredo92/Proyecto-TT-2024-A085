﻿using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Names;

namespace Name_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataNameSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[][] _names = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz .ÁÉÍÓÚáéíóú".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNameSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedName = NamesGenerator.Generate().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedName.Length / 30f);
        _names = new char[(int)arrays][];
        for (var i = 0; i < _names.Length - 1; i++)
            _names[i] = generatedName.Slice(i * 30, 30).ToArray();
        _names[^1] = generatedName[((_names.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(_names[^1].Length, 4);
        Array.Resize(ref _names[^1], minLength);
        for (var i = 0; i < _names[^1].Length; i++)
        {
            if (_names[^1][i] == '\0')
                _names[^1][i] = ' ';
        }
    }

    [Benchmark]
    public char[][] EncryptNameSerpentFpe()
    {
        var encryptedName = new char[_names.Length][];
        for (var i = 0; i < _names.Length; i++)
        {
            encryptedName[i] = _serpentFpe.Encrypt(_names[i]);
        }

        return encryptedName;
    }

    [GlobalSetup(Target = nameof(DecryptNameSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedName = NamesGenerator.Generate().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedName.Length / 30f);
        var names = new char[(int)arrays][];
        for (var i = 0; i < names.Length - 1; i++)
            names[i] = generatedName.Slice(i * 30, 30).ToArray();
        names[^1] = generatedName[((names.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(names[^1].Length, 4);
        Array.Resize(ref names[^1], minLength);
        for (var i = 0; i < names[^1].Length; i++)
        {
            if (names[^1][i] == '\0')
                names[^1][i] = ' ';
        }
        _names = new char[(int)arrays][];
        for (var i = 0; i < names.Length; i++)
        {
            _names[i] = _serpentFpe.Encrypt(names[i]);
        }
    }

    [Benchmark]
    public char[][] DecryptNameSerpentFpe()
    {
        var decryptedName = new char[_names.Length][];
        for (var i = 0; i < _names.Length; i++)
        {
            decryptedName[i] = _serpentFpe.Decrypt(_names[i]);
        }
        return decryptedName;
    }
}