using Ideological_data.Political_preferences;
using Ideological_data.Religion;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class IdeologicalDataTests(ITestOutputHelper testOutputHelper)
{
    // Test religion generator
    [Fact]
    public void TestReligionGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var religion = ReligionGenerator.Generate();
            Assert.InRange(religion.Length, 8, 32);
            testOutputHelper.WriteLine(religion);
        }
    }

    // Test political preferences generator
    [Fact]
    public void TestPoliticalPreferencesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var politicalPreferences = PoliticalPreferencesGenerator.Generate();
            Assert.InRange(politicalPreferences.Length, 3, 17);
            testOutputHelper.WriteLine(politicalPreferences);
        }
    }
}