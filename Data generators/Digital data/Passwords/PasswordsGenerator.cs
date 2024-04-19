using System.Text;

namespace Digital_data.Passwords;

public class PasswordsGenerator
{
    private const string AlphabeticChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!#$%&()*+.:;<=>?@[]^";

    public static string GeneratePassword(int length = 12)
    {
        var password = new StringBuilder();
        var random = Random.Shared;

        for (var i = 0; i < length; i++)
        {
            var charSet = random.Next(4);
            var charToAdd = charSet switch
            {
                0 => AlphabeticChars[random.Next(AlphabeticChars.Length)],
                1 => Digits[random.Next(Digits.Length)],
                2 => SpecialChars[random.Next(SpecialChars.Length)],
                3 => AlphabeticChars[random.Next(AlphabeticChars.Length)],
                _ => throw new ArgumentOutOfRangeException()
            };
            password.Append(charToAdd);
        }

        return password.ToString();
    }
}