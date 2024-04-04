using TransitAndMigratoryData.Passport_id;
using TransitAndMigratoryData.Visa;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class TransitAndMigratoryTests(ITestOutputHelper testOutputHelper)
{
    // Test passport id generator
    [Fact]
    public void TestPassportIdGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var passportId = PassportIdGenerator.GeneratePassportId();
            Assert.NotNull(passportId);
            Assert.NotEmpty(passportId);
            Assert.True(passportId.Length is 10);
            testOutputHelper.WriteLine(passportId);
        }
    }

    // Test visa generator
    [Fact]
    public void TestVisaGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var visa = VisaGenerator.GenerateVisaType();
            Assert.NotNull(visa);
            Assert.NotEmpty(visa);
            Assert.True(visa.Length == 12);
            testOutputHelper.WriteLine(visa);
        }
    }
}