using General_Data;

namespace Patrimony_data.Emission_date;

public class EmissionDateGenerator
{
    public static DateTime GenerateEmissionDate() =>
        DatesGenerator.GenerateDate(DateTime.Now, DateTime.Now.AddYears(-7));
}