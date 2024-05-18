namespace Patrimony_data.Salary;

public class SalaryGenerator
{
    public static float GenerateSalary()
    {
        return Random.Shared.NextSingle() * 10_000_000;
    }
}