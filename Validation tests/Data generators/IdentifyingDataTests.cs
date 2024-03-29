﻿using Identifying_data.Curps;
using Identifying_data.Names;
using Identifying_data.Street_names;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class IdentifyingDataFacts(ITestOutputHelper testOutputHelper)
{
    // Test names generator
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

    // Test CURP generator
    [Fact]
    public void TestCurpsGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var curp = CurpsGenerator.Generate();
            Assert.NotNull(curp);
            Assert.NotEmpty(curp);
            Assert.Equal(18, curp.Length);
            testOutputHelper.WriteLine(curp);
        }
    }

    // Test street names generator
    [Fact]
    public void TestStreetNamesGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var streetName = StreetNamesGenerator.Generate();
            Assert.NotNull(streetName);
            Assert.NotEmpty(streetName);
            testOutputHelper.WriteLine(streetName);
        }
    }
}