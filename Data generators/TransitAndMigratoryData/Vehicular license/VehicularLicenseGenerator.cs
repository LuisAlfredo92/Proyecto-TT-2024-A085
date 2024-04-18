using General_Data;

namespace Transit_and_migratory_data.Vehicular_license;

public class VehicularLicenseGenerator
{
    public static string GenerateVehicularLicense() => StringGenerator.GenerateStringWithNumbers(Random.Shared.Next(9,14));
}