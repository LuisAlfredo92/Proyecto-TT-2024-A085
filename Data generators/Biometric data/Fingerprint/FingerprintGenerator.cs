namespace Biometric_data.Fingerprint;

public class FingerprintGenerator
{
    private static readonly string[] Fingerprints = Directory.GetFiles("./Fingerprint/Images/");

    public static string GenerateFingerprint() => Fingerprints[Random.Shared.Next(0, Fingerprints.Length)];
}