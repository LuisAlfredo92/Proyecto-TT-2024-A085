using Intimate_data.Genre;
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
            var id = SexualPreferencesGenerator.GenerateId();
            Assert.InRange(id, 0, 255);

            var name = SexualPreferencesGenerator.GenerateName();
            Assert.InRange(name.Length, 7, 50);

            testOutputHelper.WriteLine($"{id} {name}");
        }
    }

    // Test genre generator
    [Fact]
    public void TestGenreGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var id = GenreGenerator.GenerateId();
            Assert.InRange(id, 0, 255);

            var name = GenreGenerator.GenerateName();
            Assert.InRange(name.Length, 4, 50);

            testOutputHelper.WriteLine($"{id} {name}");
        }
    }
}