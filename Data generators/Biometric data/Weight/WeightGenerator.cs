namespace Biometric_data.Weight;

public class WeightGenerator
{
    public static float GenerateWeight() => Random.Shared.NextSingle() * 100 + 60;
}