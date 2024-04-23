using Biometric_data.Iris;
using Biometric_data.Skin_color;
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
}