using General_Data;

namespace Academic_data.Enrolment;

public class EnrolmentGenerator
{
    public static string Generate() => StringGenerator.GenerateStringWithNumbers(Random.Shared.Next(9, 17));
}