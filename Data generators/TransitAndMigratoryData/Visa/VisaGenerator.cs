using System.Text;

namespace TransitAndMigratoryData.Visa;

public class VisaGenerator
{
    private static readonly string[] VisaTypes = ["CDJ", "MTR", "NVL", "MTM"];

    public static string GenerateVisaType()
    {
        StringBuilder sb = new();
        sb.Append(VisaTypes[Random.Shared.Next(0, VisaTypes.Length)]);
        sb.Append(Random.Shared.Next(1, 999999999).ToString().PadLeft(9, '0'));
        return sb.ToString();
    }
}