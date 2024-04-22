using System.Text.RegularExpressions;
using Academic_data.Cct;
using Academic_data.Certifications;
using Academic_data.Degrees;
using Academic_data.Referrals;
using Academic_data.Report_cards;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public partial class AcademicDataTests(ITestOutputHelper testOutputHelper)
{
    // Test academic data generator
    [Fact]
    public void TestCctGenerator()
    {
        for(var i = 0; i < 100; i++)
        {
            var cct = CctGenerator.GenerateCct();
            Assert.Equal(10, cct.Length);
            Assert.Matches(CctRegex(), cct);
            testOutputHelper.WriteLine(cct);
        }
    }
    [GeneratedRegex(@"(\d{2})([EDK])(CC|JN|PB|PB|PR|ES|TV|ST|IN|SN)(\d{4})([A-Z])")]
    private static partial Regex CctRegex();

    // Test Degree generator
    [Fact]
    public void TestDegreeGenerator()
    {
        var degree = DegreeGenerator.Generate();
        Assert.True(File.Exists(degree));
        testOutputHelper.WriteLine(degree);
    }

    // Test Referral generator
    [Fact]
    public void TestReferralGenerator()
    {
        var referral = ReferralGenerator.Generate();
        Assert.True(File.Exists(referral));
        testOutputHelper.WriteLine(referral);
    }

    // Test Report card generator
    [Fact]
    public void TestReportCardGenerator()
    {
        var reportCard = ReportCardsGenerator.Generate();
        Assert.True(File.Exists(reportCard));
        testOutputHelper.WriteLine(reportCard);
    }

    // Test Certification generator
    [Fact]
    public void TestCertificationGenerator()
    {
        var certification = CertificationsGenerator.Generate();
        Assert.True(File.Exists(certification));
        testOutputHelper.WriteLine(certification);
    }
}