using System.Security.Cryptography;
using System.Text;

namespace Academic_data.Cct;

public class CctGenerator
{
    private static readonly char[] SchoolType = ['E', 'D', 'K'];
    private static readonly string[] EducationalServiceType = ["CC", "JN", "PB", "PR", "ES", "SN", "TV", "ST", "IN"];
    public static string GenerateCct()
    {
        var cct = new StringBuilder();
        cct.Append(Random.Shared.Next(1, 33).ToString("D2"));
        cct.Append(SchoolType[Random.Shared.Next(0, SchoolType.Length)]);
        cct.Append(EducationalServiceType[Random.Shared.Next(0, EducationalServiceType.Length)]);
        cct.Append(Random.Shared.Next(1, 9999).ToString("D4"));
        cct.Append((char)Random.Shared.Next(65, 91));
        return cct.ToString();
    }
}