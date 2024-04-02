namespace Identifying_data.INE_OCR_numbers;

public class IneOcrNumbersGenerator
{
    public static long GenerateIneOcrNumber() => Random.Shared.NextInt64(1000000000000, 9999999999999);
}