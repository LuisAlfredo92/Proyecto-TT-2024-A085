namespace Biometric_data.Skin_color;

public class SkinColorGenerator
{
    public static byte GenerateFitzpatrick() => (byte)Random.Shared.Next(1, 7);

    public static byte GeneratePerla() => (byte)Random.Shared.Next(1, 12);
}