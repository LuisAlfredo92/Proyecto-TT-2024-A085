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
public class LaborDataCompanyCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[][] _companies = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCompanyCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

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
    public char[][] EncryptCompanyCast256Fpe()
    {
        var encryptedCompany = new char[_companies.Length][];
        for (var i = 0; i < _companies.Length; i++)
        {
            encryptedCompany[i] = _cast256Fpe.Encrypt(_companies[i]);
        }

        return encryptedCompany;
    }

    [GlobalSetup(Target = nameof(DecryptCompanyCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

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
            _companies[i] = _cast256Fpe.Encrypt(companies[i]);
        }
    }

    [Benchmark]
    public char[][] DecryptCompanyCast256Fpe()
    {
        var decryptedCompany = new char[_companies.Length][];
        for (var i = 0; i < _companies.Length; i++)
        {
            decryptedCompany[i] = _cast256Fpe.Decrypt(_companies[i]);
        }
        return decryptedCompany;
    }
}