using Intimate_data.Sexual_preferences;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class IntimalDataTests(ITestOutputHelper testOutputHelper)
{
    // Test sexual preferences generator
    [Fact]
    public void TestSexualPreferencesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var id = SexualPreferencesGenerator.GenerateById();
            Assert.InRange(id, 0, 255);

            var name = SexualPreferencesGenerator.GenerateByName();
            Assert.InRange(name.Length, 7, 50);

            testOutputHelper.WriteLine($"{id} {name}");
        }
    }
}