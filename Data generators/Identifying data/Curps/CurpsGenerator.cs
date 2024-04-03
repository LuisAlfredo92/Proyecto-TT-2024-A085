using System.Text;

namespace Identifying_data.Curps;

public class CurpsGenerator
{
    private static readonly string[] States =
    [
        "AS", "BC", "BS", "CC", "CS", "CH", "CL", "CM", "DF", "DG", "GT", "GR", "HG", "JC", "MC", "MS", "MN", "NT",
        "NL", "OC", "PL", "QT", "QR", "SP", "SL", "SR", "TC", "TS", "TL", "VZ", "YN", "ZS", "NE"
    ];

    private static readonly char[] Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYS".ToCharArray();
    private static readonly char[] Consonants = "BCDFGHJKLMNPQRSTVWXYZ".ToCharArray();

    public static string Generate()
    {
        var curp = new StringBuilder();
        curp.Append(GenerateName());
        curp.Append(GenerateBornDate());
        curp.Append(GenerateSex());
        curp.Append(GenerateState());
        curp.Append(GenerateConsonants());
        curp.Append(GenerateHomoclave());
        return curp.ToString();
    }

    private static string GenerateName()
    {
        var nameStringBuilder = new StringBuilder();
        Parallel.For(0, 4, _ => nameStringBuilder.Append(Letters[Random.Shared.Next(0, Letters.Length)]));
        return nameStringBuilder.ToString();
    }

    private static string GenerateBornDate()
    {
        var year = Random.Shared.Next(0, 100).ToString("00");
        var month = Random.Shared.Next(1, 13).ToString("00");
        var day = Random.Shared.Next(1, 29).ToString("00");
        return $"{year}{month}{day}";
    }

    private static string GenerateSex()
        => Random.Shared.Next(0, 2) == 1 ? "H" : "M";

    private static string GenerateState()
        => States[Random.Shared.Next(0, States.Length)];

    private static string GenerateConsonants()
    {
        var consonantsStringBuilder = new StringBuilder();
        Parallel.For(0, 3, _ => consonantsStringBuilder.Append(Consonants[Random.Shared.Next(0, Consonants.Length)]));
        return consonantsStringBuilder.ToString();
    }

    private static string GenerateHomoclave()
        => (Random.Shared.Next(0, 2) == 1 ? "A" : "0") + Random.Shared.Next(0, 10);
}