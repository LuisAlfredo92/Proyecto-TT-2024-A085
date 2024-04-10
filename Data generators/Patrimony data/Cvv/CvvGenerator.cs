namespace Patrimony_data.Cvv;

public class CvvGenerator
{
    public static int GenerateCvv()
    {
        return new Random().Next(100, 999);
    }
}