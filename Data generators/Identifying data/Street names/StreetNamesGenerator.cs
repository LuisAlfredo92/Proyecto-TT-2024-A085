namespace Identifying_data.Street_names;

public class StreetNamesGenerator
{
    private static readonly string[] Streets = File.ReadAllLines("./Street names/Streets.txt");

    public static string Generate()
        => Streets[Random.Shared.Next(0, Streets.Length)];
}