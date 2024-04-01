namespace Identifying_data.Settlements;

public class SettlementsGenerator
{
    private static readonly string[] Settlements = File.ReadAllLines("./Settlements/Settlements.txt");

    public static string Generate()
        => Settlements[Random.Shared.Next(0, Settlements.Length)];
}