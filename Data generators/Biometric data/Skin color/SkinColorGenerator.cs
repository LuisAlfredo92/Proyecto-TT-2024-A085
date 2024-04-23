namespace Biometric_data.Skin_color;

public class SkinColorGenerator
{
    private static readonly string[] _skinColors = ["#fef2f3", "#fddad9", "#f5c2b9", "#f1ca9e", "#caa27d", "#aa7b51", "#8e654a", "#7a4f38", "#573829", "#482306", "#372c23"];

    public static byte GenerateFitzpatrick() => (byte)Random.Shared.Next(1, 7);
    public static byte GeneratePerla() => (byte)Random.Shared.Next(1, 12);
    public static string GetPerlaSkinColor() => _skinColors[Random.Shared.Next(11)];
}