using General_Data;

namespace Intimate_data.Genre;

public class GenreGenerator
{
    public static byte GenerateId() => (byte)Random.Shared.Next(0, 256);

    public static string GenerateName() => StringGenerator.GenerateString(Random.Shared.Next(4, 50));
}