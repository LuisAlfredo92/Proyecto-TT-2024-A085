using General_Data;

namespace Health_data.Allergies;

public class AllergiesGenerator
{
    public static string Generate() => StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(16, 256));
}