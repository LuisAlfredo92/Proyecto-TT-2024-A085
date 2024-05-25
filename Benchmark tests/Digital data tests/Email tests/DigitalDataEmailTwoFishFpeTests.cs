using System.Security.Cryptography;
using System.Xml.Linq;
using BenchmarkDotNet.Attributes;
using Digital_data.Email;
using FPE_ciphers;
using Identifying_data.Names;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataEmailTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[][] _emails = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "abcdefghijklmnopqrstuvwxyz0123456789@_.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEmailTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        
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
    public char[][] EncryptEmailTwoFishFpe()
    {
        var encryptedEmail = new char[_emails.Length][];
        for (var i = 0; i < _emails.Length; i++)
        {
            encryptedEmail[i] = _twoFishFpe.Encrypt(_emails[i]);
        }

        return encryptedEmail;
    }

    [GlobalSetup(Target = nameof(DecryptEmailTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

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
            _emails[i] = _twoFishFpe.Encrypt(emails[i]);
        }
    }

    [Benchmark]
    public char[][] DecryptEmailTwoFishFpe()
    {
        var decryptedEmail = new char[_emails.Length][];
        for (var i = 0; i < _emails.Length; i++)
        {
            decryptedEmail[i] = _twoFishFpe.Decrypt(_emails[i]);
        }
        return decryptedEmail;
    }
}