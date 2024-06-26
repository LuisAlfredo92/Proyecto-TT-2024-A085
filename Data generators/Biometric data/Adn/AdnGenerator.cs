﻿using System.Security.Cryptography;

namespace Biometric_data.Adn;

public class AdnGenerator
{
    public static byte[] Generate()
    {
        Span<byte> adn = new byte[400_000_000];
        RandomNumberGenerator.Fill(adn);

        return adn.ToArray();
    }
}