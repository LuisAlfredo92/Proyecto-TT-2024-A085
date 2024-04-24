using System.Globalization;
using System.Text;
using Biometric_data.Adn;
using Biometric_data.Blood_type;
using Biometric_data.Fingerprint;
using Biometric_data.Iris;
using Biometric_data.Scars;
using Biometric_data.Sex;
using Biometric_data.Skin_color;
using Biometric_data.Weight;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class BiometricalDataTests(ITestOutputHelper testOutputHelper)
{
    // Test iris generator
    [Fact]
    public void TestIrisGenerator()
    {
        var filePath = IrisGenerator.Generate();
        Assert.True(File.Exists(filePath));
        testOutputHelper.WriteLine(filePath);
    }

    // Test skin color generator
    [Fact]
    public void TestSkinColorGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var fitzpatrick = SkinColorGenerator.GenerateFitzpatrick();
            Assert.InRange(fitzpatrick, 1, 6);

            var perla = SkinColorGenerator.GeneratePerla();
            Assert.InRange(perla, 1, 11);

            testOutputHelper.WriteLine($"{fitzpatrick} - {perla}");
        }
    }

    // Test scars generator
    [Fact]
    public void TestScarsGenerator()
    {
        var location = ScarsGenerator.GenerateLocation();
        Assert.InRange(location.Length, 8, 50);

        var size = ScarsGenerator.GenerateSize();
        Assert.True(size > 0);

        var filePath = ScarsGenerator.GeneratePhoto();
        Assert.True(File.Exists(filePath));
        testOutputHelper.WriteLine($"{location} - {size} - {filePath}");
    }

    // Test weight generator
    [Fact]
    public void TestWeightGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var weight = WeightGenerator.GenerateWeight();
            Assert.InRange(weight, 60, 160);

            testOutputHelper.WriteLine(weight.ToString(CultureInfo.CurrentCulture));
        }
    }

    // Test ADN generator
    [Fact]
    public void TestAdnGenerator()
    {
        for (var k = 0; k < 10; k++)
        {
            var adn = AdnGenerator.Generate();
            StringBuilder sb = new();
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    var letter = ((adn[i] >> 6 - j * 2) & 3) switch
                    {
                        0 => 'A',
                        1 => 'T',
                        2 => 'C',
                        _ => 'G'
                    };
                    sb.Append(letter);
                }
            }
            testOutputHelper.WriteLine(sb.ToString());
        }
    }

    // Test fingerprint generator
    [Fact]
    public void TestFingerprintGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var filePath = FingerprintGenerator.GenerateFingerprint();
            Assert.True(File.Exists(filePath));
            testOutputHelper.WriteLine(filePath);
        }
    }

    // Test blood type generator
    [Fact]
    public void TestBloodTypeGenerator()
    {
        string[] BloodTypes = ["A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-", "Rh nulo"];
        for (var i = 0; i < 100; i++)
        {
            var bloodType = BloodTypeGenerator.GenerateBloodType();
            Assert.Contains(bloodType, BloodTypes);

            var type = BloodTypeGenerator.GenerateType();
            Assert.InRange(type, 0, BloodTypes.Length - 1);

            var bloodTypeFromType = BloodTypeGenerator.GetBloodType(type);
            Assert.Equal(BloodTypes[type], bloodTypeFromType);

            testOutputHelper.WriteLine($"{bloodType} / {type} - {bloodTypeFromType}");
        }
    }

    // Test sex generator
    [Fact]
    public void TestSexGenerator()
    {
        string[] SexTypes = ["Masculino", "Femenino", "Otro"];
        for (var i = 0; i < 100; i++)
        {
            var bloodType = SexGenerator.GenerateSex();
            Assert.Contains(bloodType, SexTypes);

            var type = SexGenerator.GenerateSexType();
            Assert.InRange(type, 0, SexTypes.Length - 1);

            var bloodTypeFromType = SexGenerator.GetSex(type);
            Assert.Equal(SexTypes[type], bloodTypeFromType);

            testOutputHelper.WriteLine($"{bloodType} / {type} - {bloodTypeFromType}");
        }
    }
}