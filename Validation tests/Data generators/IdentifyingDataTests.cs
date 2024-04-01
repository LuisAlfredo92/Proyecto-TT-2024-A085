using Identifying_data.Curps;
using Identifying_data.House_numbers;
using Identifying_data.Interior_numbers;
using Identifying_data.Localities;
using Identifying_data.Municipalities;
using Identifying_data.Names;
using Identifying_data.Postal_code;
using Identifying_data.Settlements;
using Identifying_data.Street_names;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class IdentifyingDataFacts(ITestOutputHelper testOutputHelper)
{
    // Test names generator
    [Fact]
    public void TestNamesGenerator()
    {
        NamesGenerator namesGenerator = new();
        for (var i = 0; i < 100; i++)
        {
            var name = namesGenerator.GetRandomName();
            Assert.NotNull(name);
            Assert.NotEmpty(name);
            testOutputHelper.WriteLine(name);
        }
    }

    // Test CURP generator
    [Fact]
    public void TestCurpsGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var curp = CurpsGenerator.Generate();
            Assert.NotNull(curp);
            Assert.NotEmpty(curp);
            Assert.Equal(18, curp.Length);
            testOutputHelper.WriteLine(curp);
        }
    }

    // Test street names generator
    [Fact]
    public void TestStreetNamesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var streetName = StreetNamesGenerator.Generate();
            Assert.NotNull(streetName);
            Assert.NotEmpty(streetName);
            testOutputHelper.WriteLine(streetName);
        }
    }

    // Test house numbers generator
    [Fact]
    public void TestHouseNumbersGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var houseNumber = ExteriorNumbersGenerator.GenerateHouseNumber();
            Assert.NotNull(houseNumber);
            Assert.NotEmpty(houseNumber);
            testOutputHelper.WriteLine(houseNumber);
        }
    }

    // Test interior numbers generator
    [Fact]
    public void TestInteriorNumbersGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var interiorNumber = InteriorNumbersGenerator.GenerateInteriorNumber();
            Assert.NotNull(interiorNumber);
            Assert.NotEmpty(interiorNumber);
            testOutputHelper.WriteLine(interiorNumber);
        }
    }

    // Test settlements generator
    [Fact]
    public void TestSettlementsGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var settlement = SettlementsGenerator.Generate();
            Assert.NotNull(settlement);
            Assert.NotEmpty(settlement);
            testOutputHelper.WriteLine(settlement);
        }
    }

    // Test postal code generator
    [Fact]
    public void TestPostalCodeGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var postalCode = PostalCodeGenerator.Generate();
            Assert.NotNull(postalCode);
            Assert.NotEmpty(postalCode);
            Assert.Equal(5, postalCode.Length);
            testOutputHelper.WriteLine(postalCode);
        }
    }

    // Test localities generator
    [Fact]
    public void TestLocalitiesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var locality = LocalitiesGenerator.Generate();
            Assert.NotNull(locality);
            Assert.NotEmpty(locality);
            testOutputHelper.WriteLine(locality);
        }
    }

    // Test municipalities generator
    [Fact]
    public void TestMunicipalitiesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var municipality = MunicipalitiesGenerator.Generate();
            Assert.NotNull(municipality);
            Assert.NotEmpty(municipality);
            testOutputHelper.WriteLine(municipality);
        }
    }
}