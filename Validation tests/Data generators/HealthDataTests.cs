using System.Globalization;
using Health_data.Clinic_historical;
using Health_data.Clinic_studies;
using Health_data.Diseases;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class HealthDataTests(ITestOutputHelper testOutputHelper)
{
    // Test Diseases names generator
    [Fact]
    public void TestDiseasesNameGenerator()
    {
        for (var i = 0; i < 1000; i++)
        {
            var name = Diseases.GenerateName();
            Assert.True(name.Length <= 256);
            testOutputHelper.WriteLine(name);
        }
    }

    // Test Diagnostic date generator
    [Fact]
    public void TestDiagnosticDateGenerator()
    {
        for (var i = 0; i < 1000; i++)
        {
            var date = Diseases.GenerateDiagnosticDate();
            Assert.True(date >= DateTime.MinValue && date <= DateTime.MaxValue);
            testOutputHelper.WriteLine(date.ToString(CultureInfo.CurrentCulture));
        }
    }

    // Test clinic studies generator
    [Fact]
    public void TestClinicStudiesGenerator()
    {
        var filePath = ClinicStudiesGenerator.Generate();
        Assert.True(File.Exists(filePath));
        testOutputHelper.WriteLine(filePath);
    }
}