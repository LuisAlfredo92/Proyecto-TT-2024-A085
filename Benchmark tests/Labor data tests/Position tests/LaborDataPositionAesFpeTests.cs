using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using LaborData.Position;

namespace Position_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataPositionAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[][] _positions = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPositionAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedPosition = PositionGenerator.GeneratePosition().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedPosition.Length / 30f);
        _positions = new char[(int)arrays][];
        for (var i = 0; i < _positions.Length - 1; i++)
            _positions[i] = generatedPosition.Slice(i * 30, 30).ToArray();
        _positions[^1] = generatedPosition[((_positions.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(_positions[^1].Length, 4);
        Array.Resize(ref _positions[^1], minLength);
        for (var i = 0; i < _positions[^1].Length; i++)
        {
            if (_positions[^1][i] == '\0')
                _positions[^1][i] = ' ';
        }
    }

    [Benchmark]
    public char[][] EncryptPositionAesFpe()
    {
        var encryptedPosition = new char[_positions.Length][];
        for (var i = 0; i < _positions.Length; i++)
        {
            encryptedPosition[i] = _aesFpe.Encrypt(_positions[i]);
        }

        return encryptedPosition;
    }

    [GlobalSetup(Target = nameof(DecryptPositionAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedPosition = PositionGenerator.GeneratePosition().ToCharArray().AsSpan();
        var arrays = Math.Ceiling(generatedPosition.Length / 30f);
        var companies = new char[(int)arrays][];
        for (var i = 0; i < companies.Length - 1; i++)
            companies[i] = generatedPosition.Slice(i * 30, 30).ToArray();
        companies[^1] = generatedPosition[((companies.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(companies[^1].Length, 4);
        Array.Resize(ref companies[^1], minLength);
        for (var i = 0; i < companies[^1].Length; i++)
        {
            if (companies[^1][i] == '\0')
                companies[^1][i] = ' ';
        }
        _positions = new char[(int)arrays][];
        for (var i = 0; i < companies.Length; i++)
        {
            _positions[i] = _aesFpe.Encrypt(companies[i]);
        }
    }

    [Benchmark]
    public char[][] DecryptPositionAesFpe()
    {
        var decryptedPosition = new char[_positions.Length][];
        for (var i = 0; i < _positions.Length; i++)
        {
            decryptedPosition[i] = _aesFpe.Decrypt(_positions[i]);
        }
        return decryptedPosition;
    }
}