namespace Identifying_data.Phone_numbers;

public class PhoneNumbersGenerator
{
    public static long GeneratePhoneNumber() => Random.Shared.NextInt64(1000000000, 999999999999999);
}