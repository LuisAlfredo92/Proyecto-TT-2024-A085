using System.Text;

namespace Identifying_data.Military_service_number;

public class MilitaryServiceNumbersGenerator
{
    public static string GenerateMilitaryServiceNumber()
    {
        var random = Random.Shared;
        var militaryServiceNumber = new StringBuilder();
        militaryServiceNumber.Append((char)random.Next(65, 91));
        militaryServiceNumber.Append('-');
        militaryServiceNumber.Append(random.Next());
        return militaryServiceNumber.ToString();
    }
}