namespace Biometric_data.Sex;

public class SexGenerator
{
    private static readonly string[] SexTypes = ["Masculino", "Femenino", "Otro"];

    public static string GenerateSex() => SexTypes[Random.Shared.Next(0, SexTypes.Length)];

    public static byte GenerateSexType() => (byte)Random.Shared.Next(0, SexTypes.Length);

    public static string GetSex(byte type) => SexTypes[type];
}