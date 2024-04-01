namespace Identifying_data.Postal_code;

public class PostalCodeGenerator
{
    public static string Generate()
    {
        var random = new Random();
        var postalCode = random.Next(1000, 99999).ToString().PadLeft(5, '0');
        return postalCode;
    }
}