using Biometric_data.Iris;
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
}