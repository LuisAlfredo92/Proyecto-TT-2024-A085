using General_Data;

namespace Intimate_data.Sexual_preferences;

public class SexualPreferencesGenerator
{
    public static byte GenerateById() => (byte)Random.Shared.Next(0, 256);

    public static string GenerateByName() => StringGenerator.GenerateString(Random.Shared.Next(7, 50));
}