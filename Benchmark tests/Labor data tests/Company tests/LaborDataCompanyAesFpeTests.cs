using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using LaborData.Company;

namespace Company_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataCompanyAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[][] _companies = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCompanyAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedCompany = CompanyGenerator.GenerateCompanyName().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedCompany.Length / 30f);
        _companies = new char[(int)arrays][];
        for (var i = 0; i < _companies.Length - 1; i++)
            _companies[i] = generatedCompany.Slice(i * 30, 30).ToArray();
        _companies[^1] = generatedCompany[((_companies.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(_companies[^1].Length, 4);
        Array.Resize(ref _companies[^1], minLength);
        for (var i = 0; i < _companies[^1].Length; i++)
        {
            if (_companies[^1][i] == '\0')
                _companies[^1][i] = ' ';
        }
    }

    [Benchmark]
    public char[][] EncryptCompanyAesFpe()
    {
        var encryptedCompany = new char[_companies.Length][];
        for (var i = 0; i < _companies.Length; i++)
        {
            encryptedCompany[i] = _aesFpe.Encrypt(_companies[i]);
        }

        return encryptedCompany;
    }

    [GlobalSetup(Target = nameof(DecryptCompanyAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedCompany = CompanyGenerator.GenerateCompanyName().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedCompany.Length / 30f);
        var companies = new char[(int)arrays][];
        for (var i = 0; i < companies.Length - 1; i++)
            companies[i] = generatedCompany.Slice(i * 30, 30).ToArray();
        companies[^1] = generatedCompany[((companies.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(companies[^1].Length, 4);
        Array.Resize(ref companies[^1], minLength);
        for (var i = 0; i < companies[^1].Length; i++)
        {
            if (companies[^1][i] == '\0')
                companies[^1][i] = ' ';
        }
        _companies = new char[(int)arrays][];
        for (var i = 0; i < companies.Length; i++)
        {
            _companies[i] = _aesFpe.Encrypt(companies[i]);
        }
    }

    [Benchmark]
    public char[][] DecryptCompanyAesFpe()
    {
        var decryptedCompany = new char[_companies.Length][];
        for (var i = 0; i < _companies.Length; i++)
        {
            decryptedCompany[i] = _aesFpe.Decrypt(_companies[i]);
        }
        return decryptedCompany;
    }
}