namespace Intimate_data.Marital_status;

public class MaritalStatusGenerator
{
    public static int Generate() => Random.Shared.Next(1, 7);
}