using General_Data;

namespace TransitAndMigratoryData.Passport_id;

public class PassportIdGenerator
{
    public static string GeneratePassportId()
    {
        var length = Random.Shared.Next(3, 11);
        var id = StringGenerator.GenerateStringWithNumbers(length);
        return id.PadLeft(10, '0');
    }
}