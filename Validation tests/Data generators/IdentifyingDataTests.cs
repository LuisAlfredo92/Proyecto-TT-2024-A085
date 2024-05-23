using System.Globalization;
using Identifying_data.Born_dates;
using Identifying_data.Curps;
using Identifying_data.Exterior_numbers;
using Identifying_data.INE_CIC_numbers;
using Identifying_data.INE_OCR_numbers;
using Identifying_data.Interior_numbers;
using Identifying_data.Localities;
using Identifying_data.Military_service_number;
using Identifying_data.Municipalities;
using Identifying_data.Names;
using Identifying_data.Phone_numbers;
using Identifying_data.Postal_code;
using Identifying_data.Rfc;
using Identifying_data.Settlements;
using Identifying_data.States;
using Identifying_data.Street_names;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class IdentifyingDataFacts(ITestOutputHelper testOutputHelper)
{
    // Test names generator
    [Fact]
    public void TestNamesGenerator()
    {
        for (var i = 0; i < 1_000_000; i++)
        {
            var name = NamesGenerator.Generate();
            testOutputHelper.WriteLine(name);
            Assert.NotNull(name);
            Assert.NotEmpty(name);
            Assert.True(name.Length >= 1);
            Assert.Matches("[ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz .ÁÉÍÓÚáéíóú]", name);
        }
    }

    // Test born dates generator
    [Fact]
    public void TestBornDatesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var bornDate = BornDatesGenerator.GenerateBornDate();
            Assert.True(bornDate >= DateTime.MinValue && bornDate <= DateTime.Today);
            testOutputHelper.WriteLine(bornDate.ToString(CultureInfo.CurrentCulture));
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

    // Test CIC INE generator
    [Fact]
    public void TestCicIneGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var cicIne = IneCicNumbersGenerator.GenerateIneCicNumber();
            Assert.True(cicIne is >= 1000000000 and <= 9999999999);
            testOutputHelper.WriteLine(cicIne.ToString());
        }
    }

    // Test OCR INE generator
    [Fact]
    public void TestOcrIneGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var ocrIne = IneOcrNumbersGenerator.GenerateIneOcrNumber();
            Assert.True(ocrIne is >= 1000000000000 and <= 9999999999999);
            testOutputHelper.WriteLine(ocrIne.ToString());
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

    // Test states generator
    [Fact]
    public void TestStatesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var state = StatesGenerator.Generate();
            Assert.NotNull(state);
            Assert.NotEmpty(state);
            testOutputHelper.WriteLine(state);
        }
    }

    // Test RFC generator
    [Fact]
    public void TestRfcGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var rfc = RfcGenerator.Generate();
            Assert.NotNull(rfc);
            Assert.NotEmpty(rfc);
            Assert.Equal(13, rfc.Length);
            testOutputHelper.WriteLine(rfc);
        }
    }

    // Test phone number generator
    [Fact]
    public void TestPhoneNumberGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var phoneNumber = PhoneNumbersGenerator.GeneratePhoneNumber();
            Assert.True(phoneNumber is >= 1000000000 and <= 999999999999999);
            testOutputHelper.WriteLine(phoneNumber.ToString());
        }
    }

    // Test military service number generator
    [Fact]
    public void TestMilitaryServiceNumberGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var militaryServiceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber();
            Assert.NotNull(militaryServiceNumber);
            Assert.NotEmpty(militaryServiceNumber);
            testOutputHelper.WriteLine(militaryServiceNumber);
        }
    }
}