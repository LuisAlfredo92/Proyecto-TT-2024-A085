using General_Data;

namespace LaborData.Company;

public class CompanyGenerator
{
    public static string GenerateCompanyName()
    {
        var length = Random.Shared.Next(20, 65);
        return StringGenerator.GenerateString(length);
    }
}