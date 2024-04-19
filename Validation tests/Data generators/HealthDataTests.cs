using System.Globalization;
using Health_data.Allergies;
using Health_data.Clinic_historical;
using Health_data.Clinic_studies;
using Health_data.Diseases;
using Health_data.Nss;
using Health_data.Psychological_condition;
using Health_data.Treatments;
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

    // Test psychological condition generator
    [Fact]
    public void TestPsychologicalConditionGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var condition = PsychologicalConditionGenerator.Generate();
            Assert.True(condition.Length <= 256);
            testOutputHelper.WriteLine(condition);
        }
    }

    // Test clinic historical generator
    [Fact]
    public void TestClinicHistoricalGenerator()
    {
        var filePath = ClinicHistoricalGenerator.GenerateStudies();
        Assert.True(File.Exists(filePath));
        testOutputHelper.WriteLine(filePath);
    }

    // Test treatments generator
    [Fact]
    public void TestTreatmentsGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var treatment = TreatmentsGenerator.Generate();
            Assert.True(treatment.Length <= 256);
            testOutputHelper.WriteLine(treatment);
        }
    }

    // Test allergies generator
    [Fact]
    public void TestAllergiesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var allergy = AllergiesGenerator.Generate();
            Assert.True(allergy.Length <= 256);
            testOutputHelper.WriteLine(allergy);
        }
    }

    // Test NSS generator
    [Fact]
    public void TestNssGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var nss = NssGenerator.Generate();
            Assert.True(nss >= 10000000000 && nss <= 99999999999);
            testOutputHelper.WriteLine(nss.ToString());
        }
    }
}