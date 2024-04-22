using General_Data;

namespace Ideological_data.Union_affiliation;

public class UnionAffiliationGenerator
{
    public static int GenerateId() => Random.Shared.Next(1, 10000);

    public static string GenerateName() => StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(32, 200));
}