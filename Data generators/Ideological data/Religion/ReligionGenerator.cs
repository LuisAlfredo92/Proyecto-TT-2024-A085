using General_Data;

namespace Ideological_data.Religion;

public class ReligionGenerator
{
    public static string Generate() => StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(8, 32));
}