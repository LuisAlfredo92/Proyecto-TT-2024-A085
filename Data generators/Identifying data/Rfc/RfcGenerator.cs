using System.Text;

namespace Identifying_data.Rfc;

public class RfcGenerator
{
    private const string Vowels = "AEIOU";
    private const string Consonants = "BCDFGHJKLMNPQRSTVWXYZ";

    public static string Generate()
    {
        var rfc = new StringBuilder();
        rfc.Append(GenerateName());
        rfc.Append(GenerateDate());
        rfc.Append(GenerateHomoclave());
        return rfc.ToString();
    }

    private static string GenerateName()
    {
        var name = new StringBuilder();
        name.Append(Consonants[Random.Shared.Next(Consonants.Length)]);
        name.Append(Vowels[Random.Shared.Next(Vowels.Length)]);
        name.Append(Consonants[Random.Shared.Next(Consonants.Length)]);
        name.Append(Consonants[Random.Shared.Next(Consonants.Length)]);
        
        return name.ToString();
    }

    private static string GenerateDate()
    {
        var date = new StringBuilder();
        date.Append(Random.Shared.Next(100).ToString("D2"));
        date.Append(Random.Shared.Next(1, 13).ToString("D2"));
        date.Append(Random.Shared.Next(1, 32).ToString("D2"));
        
        return date.ToString();
    }

    private static string GenerateHomoclave()
    {
        var homoclave = new StringBuilder();
        homoclave.Append(Consonants[Random.Shared.Next(Consonants.Length)]);
        homoclave.Append(Vowels[Random.Shared.Next(Vowels.Length)]);
        homoclave.Append(Random.Shared.Next(10));
        
        return homoclave.ToString();
    }
}