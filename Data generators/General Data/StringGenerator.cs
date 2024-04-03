using System.Text;

namespace General_Data;

public class StringGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    public static string GenerateString(int length)
    {
        StringBuilder sb = new();
        Parallel.For(0, length, _ => sb.Append(Chars[Random.Shared.Next(Chars.Length)]));
        return sb.ToString();
    }
}