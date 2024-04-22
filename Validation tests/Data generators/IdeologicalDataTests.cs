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
}