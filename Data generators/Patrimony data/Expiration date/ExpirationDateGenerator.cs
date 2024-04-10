using General_Data;

namespace Patrimony_data.Expiration_date;

public class ExpirationDateGenerator
{
    public static DateTime GenerateExpirationDate()
    {
        return DatesGenerator.GenerateDate(DateTime.Now, DateTime.Now.AddYears(5));
    }
}