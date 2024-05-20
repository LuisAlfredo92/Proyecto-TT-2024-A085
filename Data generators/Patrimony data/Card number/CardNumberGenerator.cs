namespace Patrimony_data.Card_number;

public class CardNumberGenerator
{
    public static long GenerateCardNumber()
        => Random.Shared.NextInt64(1_000_000_000_000_000, 9_999_999_999_999_999);
}