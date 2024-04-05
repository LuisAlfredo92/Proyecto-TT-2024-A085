using General_Data;

namespace TransitAndMigratoryData.Vehicular_license;

public class VehicularLicenseGenerator
{
    public static string GenerateVehicularLicense() => StringGenerator.GenerateStringWithNumbers(Random.Shared.Next(9,14));
}