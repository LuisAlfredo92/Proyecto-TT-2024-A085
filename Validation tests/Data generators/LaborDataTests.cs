using LaborData.Company;
using LaborData.Position;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class LaborDataTests(ITestOutputHelper testOutputHelper)
{
    // Test company name generator
    [Fact]
    public void TestCompanyNameGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var companyName = CompanyGenerator.GenerateCompanyName();
            Assert.NotNull(companyName);
            Assert.NotEmpty(companyName);
            Assert.True(companyName.Length is >= 20 and <= 64);
            testOutputHelper.WriteLine(companyName);
        }
    }

    // Test position generator
    [Fact]
    public void TestPositionGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var position = PositionGenerator.GeneratePosition();
            Assert.NotNull(position);
            Assert.NotEmpty(position);
            Assert.True(position.Length is >= 2 and <= 64);
            testOutputHelper.WriteLine(position);
        }
    }
}