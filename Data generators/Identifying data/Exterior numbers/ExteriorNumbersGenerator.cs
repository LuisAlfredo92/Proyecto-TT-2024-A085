using System.Text;

namespace Identifying_data.Exterior_numbers;

public class ExteriorNumbersGenerator
{
    //125, 1098, 572-A, Domicilio
    // Conocido, Manzana 15, Lote
    // 23.

    public static string GenerateHouseNumber()
    {
        var houseNumber = new StringBuilder();
        var random = Random.Shared;

        var houseNumberType = random.Next(0, 3);
        switch (houseNumberType)
        {
            case 0:
                houseNumber.Append(random.Next(1, 10000));

                if (random.Next(0, 2) == 1)
                {
                    houseNumber.Append('-');
                    houseNumber.Append((char) random.Next(65, 91));
                }
                break;
            case 1:
                houseNumber.Append(random.Next(0, 2) == 1 ? "Mz. " : "Manzana ");
                houseNumber.Append(random.Next(1, 100));
                break;
            case 2:
                houseNumber.Append(random.Next(0, 2) == 1 ? "Lt. " : "Lote ");
                houseNumber.Append(random.Next(1, 100));
                break;
        }

        return houseNumber.ToString();
    }
}