namespace Patrimony_data.Card_number;

public class CardNumberGenerator
{
    public static long GenerateCardNumber()
        => Random.Shared.NextInt64(1000000000000000, 9999999999999999);
}