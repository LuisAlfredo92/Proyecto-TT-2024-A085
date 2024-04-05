using System.Text.RegularExpressions;
using TransitAndMigratoryData.Automobile_license_plate;
using TransitAndMigratoryData.Niv;
using TransitAndMigratoryData.Passport_id;
using TransitAndMigratoryData.Vehicular_license;
using TransitAndMigratoryData.Visa;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public partial class TransitAndMigratoryTests(ITestOutputHelper testOutputHelper)
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

    // Test NIV generator
    [Fact]
    public void TestNivGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var niv = NivGenerator.GenerateNiv();
            Assert.NotNull(niv);
            Assert.NotEmpty(niv);
            Assert.True(niv.Length == 17);
            testOutputHelper.WriteLine(niv);
        }
    }

    // Test vehicular license generator
    [Fact]
    public void TestVehicularLicenseGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var vehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense();
            Assert.NotNull(vehicularLicense);
            Assert.NotEmpty(vehicularLicense);
            Assert.True(vehicularLicense.Length >= 9 && vehicularLicense.Length <= 14);
            testOutputHelper.WriteLine(vehicularLicense);
        }
    }

    // Test automobile license plate generator
    [Fact]
    public void TestAutomobileLicensePlateGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var automobileLicensePlate = AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate();
            Assert.NotNull(automobileLicensePlate);
            Assert.NotEmpty(automobileLicensePlate);
            Assert.True(automobileLicensePlate.Length is >= 7 and <= 9);
            Assert.Matches(AutomobileLicensePlateRegex(), automobileLicensePlate);
            testOutputHelper.WriteLine(automobileLicensePlate);
        }
    }

    [GeneratedRegex("([A-Z]{1,3}-)?[0-9]{2,3}-([0-9]{2}|[A-Z]{1,3})")]
    private static partial Regex AutomobileLicensePlateRegex();
}