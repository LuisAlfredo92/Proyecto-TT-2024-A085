using System.Text;

namespace Identifying_data.Interior_numbers;

public class InteriorNumbersGenerator
{
    // 2, Local C,
    // L-5

    public static string GenerateInteriorNumber()
    {
        var interiorNumber = new StringBuilder();
        var random = Random.Shared;

        var interiorNumberType = random.Next(0, 2);
        switch (interiorNumberType)
        {
            case 0:
                interiorNumber.Append(random.Next(1, 100));
                break;
            case 1:
                interiorNumber.Append(random.Next(0, 2) == 1 ? "Local " : "L-");
                interiorNumber.Append(random.Next(1, 100));
                break;
        }

        return interiorNumber.ToString();
    }
}