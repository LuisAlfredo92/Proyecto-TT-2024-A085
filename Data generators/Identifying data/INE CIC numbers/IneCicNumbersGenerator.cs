namespace Identifying_data.INE_CIC_numbers;

public class IneCicNumbersGenerator
{
    public static long GenerateIneCicNumber() => Random.Shared.NextInt64(1000000000, 9999999999);
}