namespace Identifying_data.Names;

public class NamesGenerator
{
    private static readonly string[] LastNames = File.ReadAllLines("./Names/Apellidos.txt");
    private static readonly string[] Names = [.. File.ReadAllLines("./Names/Hombres.txt"), .. File.ReadAllLines("./Names/Mujeres.txt")];

    public static string Generate()
        => $"{Names[Random.Shared.Next(Names.Length)]} {LastNames[Random.Shared.Next(LastNames.Length)]} {LastNames[Random.Shared.Next(LastNames.Length)]}";

    public static string GetName()
        => Names[Random.Shared.Next(Names.Length)];

    public static string GetLastName()
        => LastNames[Random.Shared.Next(LastNames.Length)];
}