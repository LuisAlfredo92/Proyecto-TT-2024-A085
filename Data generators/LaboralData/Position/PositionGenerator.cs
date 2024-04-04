using General_Data;

namespace LaborData.Position;

public class PositionGenerator
{
    public static string GeneratePosition()
    {
        var length = Random.Shared.Next(2, 65);
        return StringGenerator.GenerateString(length);
    }
}