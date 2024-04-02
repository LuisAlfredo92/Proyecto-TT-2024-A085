namespace General_Data;

public class NumbersGenerator
{
    public static int GenerateNumber(int min, int max) => Random.Shared.Next(min, max);
}