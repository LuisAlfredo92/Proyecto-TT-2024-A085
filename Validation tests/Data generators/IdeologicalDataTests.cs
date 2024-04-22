using Ideological_data.Civil_organizations;
using Ideological_data.Political_preferences;
using Ideological_data.Religion;
using Ideological_data.Union_affiliation;
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

    // Test union affiliation generator
    [Fact]
    public void TestUnionAffiliationGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var id = UnionAffiliationGenerator.GenerateId();
            Assert.InRange(id, 1, 10000);
            var name = UnionAffiliationGenerator.GenerateName();
            Assert.InRange(name.Length, 32, 200);
            testOutputHelper.WriteLine($"{id} {name}");
        }
    }

    // Test civil organizations generator
    [Fact]
    public void TestCivilOrganizationsGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var civilOrganization = CivilOrganizationsGenerator.Generate();
            Assert.Equal(14, civilOrganization.Length);
            testOutputHelper.WriteLine(civilOrganization);
        }
    }
}