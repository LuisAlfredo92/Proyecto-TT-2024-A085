using General_Data;

namespace Patrimony_data.Afore;

public class AforeGenerator
{
    public static string GenerateAforeName() => StringGenerator.GenerateString(Random.Shared.Next(4, 17));
}