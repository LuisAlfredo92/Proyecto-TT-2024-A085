using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Identifying_data.Names;

public class NamesGenerator
{
    private readonly string[] _lastNames;
    private readonly string[] _names;

    public NamesGenerator()
    {
        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
        };

        // Last names
        List<string> lastNames = [];
        using var lastNamesStreamReader = File.OpenText("./Names/apellidos.csv");
        using var lastNamesReader = new CsvReader(lastNamesStreamReader, csvConfiguration);
        while (lastNamesReader.Read())
            lastNames.Add(lastNamesReader.GetField(0));
        _lastNames = [.. lastNames];

        // Men names
        using var menStreamReader = File.OpenText("./Names/hombres.csv");
        List<string> names = [];
        using var menReader = new CsvReader(menStreamReader, csvConfiguration);
        menReader.Read();
        while (menReader.Read())
            names.Add(menReader.GetField(0));

        // Women names
        using var womenStreamReader = File.OpenText("./Names/mujeres.csv");
        using var womenReader = new CsvReader(womenStreamReader, csvConfiguration);
        womenReader.Read();
        while (womenReader.Read())
            names.Add(womenReader.GetField(0));

        _names = [.. names];
    }

    public string GetRandomName()
        => $"{_names[Random.Shared.Next(_names.Length)]} {_lastNames[Random.Shared.Next(_lastNames.Length)]} {_lastNames[Random.Shared.Next(_lastNames.Length)]}";
}