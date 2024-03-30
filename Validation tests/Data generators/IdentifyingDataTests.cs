using Identifying_data.Curps;
using Identifying_data.House_numbers;
using Identifying_data.Interior_numbers;
using Identifying_data.Names;
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
}