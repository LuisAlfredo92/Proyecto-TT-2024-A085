using General_Data;

namespace Health_data.Treatments;

public class TreatmentsGenerator
{
    public static string Generate() => StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(16, 256));
}