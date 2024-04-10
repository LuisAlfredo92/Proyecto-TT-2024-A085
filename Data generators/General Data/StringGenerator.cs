using System.Text;

namespace General_Data;

public class StringGenerator
{
    private const string CharsWithNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public static string GenerateStringWithNumbers(int length)
    {
        StringBuilder sb = new();
        for (var i = 0; i < length; i++)
            sb.Append(CharsWithNumbers[Random.Shared.Next(CharsWithNumbers.Length)]);
        return sb.ToString();
    }

    public static string GenerateString(int length)
    {
        StringBuilder sb = new();
        for(var i = 0; i < length; i++)
            sb.Append(Chars[Random.Shared.Next(Chars.Length)]);
        return sb.ToString();
    }

    public static string GenerateStringWithSpaces(int length)
    {
        StringBuilder sb = new();
        for (var i = 0; i < length; i++)
            sb.Append(Random.Shared.NextSingle() < 0.25f ? ' ' : Chars[Random.Shared.Next(Chars.Length)]);
        return sb.ToString();
    }

    public static string GenerateArbitraryString(int length)
    {
        StringBuilder sb = new();
        for (var i = 0; i < length; i++)
            sb.Append((char)Random.Shared.Next(32, 256));
        return sb.ToString();
    }
}