using System.Text;

namespace Digital_data.Usernames;

public class UsernamesGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-";

    public static string GenerateUsername()
    {
        var length = Random.Shared.Next(6, 30);
        StringBuilder sb = new();
        Parallel.For(0, length, _ =>
        {
            sb.Append(Chars[Random.Shared.Next(Chars.Length)]);
        });
        return sb.ToString();
    }
}