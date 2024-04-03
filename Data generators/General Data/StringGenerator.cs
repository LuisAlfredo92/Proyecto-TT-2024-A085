using System.Text;

namespace General_Data;

public class StringGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    public static string GenerateString(int length)
    {
        StringBuilder sb = new();
        for (var i = 0; i < length; i++)
            sb.Append(Chars[Random.Shared.Next(Chars.Length)]);
        return sb.ToString();
    }
}