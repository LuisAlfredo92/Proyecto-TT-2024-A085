namespace Patrimony_data.Investment_return;

public class InvestmentReturnGenerator
{
    public static float GenerateInvestmentReturn() =>
        Random.Shared.NextSingle() * 100;
}