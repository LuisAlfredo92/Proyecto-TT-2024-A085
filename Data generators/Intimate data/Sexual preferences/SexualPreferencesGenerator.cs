using General_Data;

namespace Intimate_data.Sexual_preferences;

public class SexualPreferencesGenerator
{
    public static byte GenerateId() => (byte)Random.Shared.Next(0, 256);

    public static string GenerateName() => StringGenerator.GenerateString(Random.Shared.Next(7, 50));
}