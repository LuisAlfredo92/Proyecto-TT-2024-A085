﻿using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Digital_data.Email;
using FPE_ciphers;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataEmailCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[][] _emails = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "abcdefghijklmnopqrstuvwxyz0123456789@_.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEmailCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedEmail = EmailGenerator.GenerateEmail().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedEmail.Length / 30f);
        _emails = new char[(int)arrays][];
        for (var i = 0; i < _emails.Length - 1; i++)
            _emails[i] = generatedEmail.Slice(i * 30, 30).ToArray();
        _emails[^1] = generatedEmail[((_emails.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(_emails[^1].Length, 4);
        Array.Resize(ref _emails[^1], minLength);
        for (var i = 0; i < _emails[^1].Length; i++)
        {
            if (_emails[^1][i] == '\0')
                _emails[^1][i] = '_';
        }
    }

    [Benchmark]
    public char[][] EncryptEmailCast256Fpe()
    {
        var encryptedEmail = new char[_emails.Length][];
        for (var i = 0; i < _emails.Length; i++)
        {
            encryptedEmail[i] = _cast256Fpe.Encrypt(_emails[i]);
        }

        return encryptedEmail;
    }

    [GlobalSetup(Target = nameof(DecryptEmailCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedEmail = EmailGenerator.GenerateEmail().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedEmail.Length / 30f);
        var emails = new char[(int)arrays][];
        for (var i = 0; i < emails.Length - 1; i++)
            emails[i] = generatedEmail.Slice(i * 30, 30).ToArray();
        emails[^1] = generatedEmail[((emails.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(emails[^1].Length, 4);
        Array.Resize(ref emails[^1], minLength);
        for (var i = 0; i < emails[^1].Length; i++)
        {
            if (emails[^1][i] == '\0')
                emails[^1][i] = '_';
        }
        _emails = new char[(int)arrays][];
        for (var i = 0; i < emails.Length; i++)
        {
            _emails[i] = _cast256Fpe.Encrypt(emails[i]);
        }
    }

    [Benchmark]
    public char[][] DecryptEmailCast256Fpe()
    {
        var decryptedEmail = new char[_emails.Length][];
        for (var i = 0; i < _emails.Length; i++)
        {
            decryptedEmail[i] = _cast256Fpe.Decrypt(_emails[i]);
        }
        return decryptedEmail;
    }
}