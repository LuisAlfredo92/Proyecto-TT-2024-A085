namespace Identifying_data.Municipalities;

public class MunicipalitiesGenerator
{
    private static readonly string[] Municipalities = File.ReadAllLines("./Municipalities/Municipalities.txt");

    public static string Generate() => Municipalities[new Random().Next(Municipalities.Length)];
}