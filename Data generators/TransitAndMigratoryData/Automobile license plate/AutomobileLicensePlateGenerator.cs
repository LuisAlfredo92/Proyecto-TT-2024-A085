using System.Text;
using General_Data;

namespace TransitAndMigratoryData.Automobile_license_plate;

public class AutomobileLicensePlateGenerator
{
    public static string GenerateAutomobileLicensePlate()
    {
        var plateType = Random.Shared.Next(0, 5);
        StringBuilder licensePlate = new();

        switch (plateType)
        {
            // AAA-00-00
            case 0:
                licensePlate.Append(StringGenerator.GenerateString(3).ToUpper());
                licensePlate.Append('-');
                licensePlate.Append(Random.Shared.Next(0, 100).ToString("D2"));
                licensePlate.Append('-');
                licensePlate.Append(Random.Shared.Next(0, 100).ToString("D2"));
                break;

            // AAA-000-A
            case 1:
                licensePlate.Append(StringGenerator.GenerateString(3).ToUpper());
                licensePlate.Append('-');
                licensePlate.Append(Random.Shared.Next(0, 1000).ToString("D3"));
                licensePlate.Append('-');
                licensePlate.Append(StringGenerator.GenerateString(1).ToUpper());
                break;

            // A-000-AAA
            case 2:
                licensePlate.Append(StringGenerator.GenerateString(1).ToUpper());
                licensePlate.Append('-');
                licensePlate.Append(Random.Shared.Next(0, 1000).ToString("D3"));
                licensePlate.Append('-');
                licensePlate.Append(StringGenerator.GenerateString(3).ToUpper());
                break;

            // 000-AAA
            case 3:
                licensePlate.Append(Random.Shared.Next(0, 1000).ToString("D3"));
                licensePlate.Append('-');
                licensePlate.Append(StringGenerator.GenerateString(3).ToUpper());
                break;

            // A-00-AAA
            case 4:
                licensePlate.Append(StringGenerator.GenerateString(1).ToUpper());
                licensePlate.Append('-');
                licensePlate.Append(Random.Shared.Next(0, 100).ToString("D2"));
                licensePlate.Append('-');
                licensePlate.Append(StringGenerator.GenerateString(3).ToUpper());
                break;
        }
        return licensePlate.ToString();
    }
}