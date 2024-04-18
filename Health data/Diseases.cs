using General_Data;

namespace Health_data;

public class Diseases
{
    public static string GenerateName() => StringGenerator.GenerateStringWithSpaces(256);
    public static DateTime GenerateDiagnosticDate() => DatesGenerator.GenerateDate(DateTime.MinValue, DateTime.MaxValue);
}