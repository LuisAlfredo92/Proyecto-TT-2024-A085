namespace Biometric_data.Blood_type;

public class BloodTypeGenerator
{
    private static readonly string[] BloodTypes = ["A+","A-", "B+", "B-", "AB+", "AB-", "O+", "O-", "Rh nulo"];

    public static string GenerateBloodType() => BloodTypes[Random.Shared.Next(0, BloodTypes.Length)];

    public static byte GenerateType() => (byte) Random.Shared.Next(0, BloodTypes.Length);

    public static string GetBloodType(byte type) => BloodTypes[type];
}