namespace Biometric_data.Height;

public class HeightGenerator
{
    public static int GenerateHeightInCentimeters() => Random.Shared.Next(150, 200);

    public static float GenerateHeightInMeters() => Random.Shared.NextSingle() * 0.6f + 1.5f;
}