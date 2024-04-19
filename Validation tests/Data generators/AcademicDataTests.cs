﻿using System.Text.RegularExpressions;
using Academic_data.Cct;
using Academic_data.Degrees;
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

    // Test Degree generator
    [Fact]
    public void TestDegreeGenerator()
    {
        var degree = DegreeGenerator.Generate();
        Assert.True(File.Exists(degree));
        testOutputHelper.WriteLine(degree);
    }

    [GeneratedRegex(@"(\d{2})([EDK])(CC|JN|PB|PB|PR|ES|TV|ST|IN|SN)(\d{4})([A-Z])")]
    private static partial Regex CctRegex();
}