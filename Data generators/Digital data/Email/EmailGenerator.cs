using System.Text;

namespace Digital_data.Email;

public class EmailGenerator
{
    public static string GenerateEmail()
    {
        StringBuilder email = new();
        int domainLength = Random.Shared.Next(12, 256),
            localLength = Random.Shared.Next(5, 60);
        for (var i = 0; i < domainLength; i++)
            email.Append((char)Random.Shared.Next(97, 123));
        email.Append('@');
        for (var i = 0; i < localLength; i++)
            email.Append((char)Random.Shared.Next(97, 123));
        email.Append('.');
        for (var i = 0; i < 3; i++)
            email.Append((char)Random.Shared.Next(97, 123));
        return email.ToString();
    }
}