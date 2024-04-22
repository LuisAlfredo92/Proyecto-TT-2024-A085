using General_Data;

namespace Ideological_data.Political_preferences;

public class PoliticalPreferencesGenerator
{
    public static string Generate() => StringGenerator.GenerateString(Random.Shared.Next(3, 17));
}