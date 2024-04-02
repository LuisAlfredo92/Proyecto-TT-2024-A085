using General_Data;

namespace Identifying_data.Born_dates;

public class BornDatesGenerator
{
    public static DateTime GenerateBornDate()
    {
        return DatesGenerator.GenerateDate(DateTime.MinValue, DateTime.Today);
    }
}