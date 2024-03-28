using Identifying_data;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class IdentifyingDataTests(ITestOutputHelper testOutputHelper)
{
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
}