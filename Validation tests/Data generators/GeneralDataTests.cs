using System.Globalization;
using General_Data;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class GeneralDataTests(ITestOutputHelper testOutputHelper)
{
    // Test date generator
    [Fact]
    public void TestDateGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var date = DatesGenerator.GenerateDate(DateTime.MinValue, DateTime.Now);
            Assert.True(date >= DateTime.MinValue && date <= DateTime.Today);
            testOutputHelper.WriteLine(date.ToString(CultureInfo.CurrentCulture));
        }
    }

    // Test number generator
    [Fact]
    public void TestNumberGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var number = NumbersGenerator.GenerateNumber(0, 100);
            Assert.True(number is >= 0 and <= 100);
            testOutputHelper.WriteLine(number.ToString(CultureInfo.CurrentCulture));
        }
    }
}