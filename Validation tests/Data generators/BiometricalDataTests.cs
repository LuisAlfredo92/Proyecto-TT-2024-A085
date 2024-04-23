﻿using System.Globalization;
using Biometric_data.Iris;
using Biometric_data.Scars;
using Biometric_data.Skin_color;
using Biometric_data.Weight;
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

    // Test skin color generator
    [Fact]
    public void TestSkinColorGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var fitzpatrick = SkinColorGenerator.GenerateFitzpatrick();
            Assert.InRange(fitzpatrick, 1, 6);

            var perla = SkinColorGenerator.GeneratePerla();
            Assert.InRange(perla, 1, 11);

            testOutputHelper.WriteLine($"{fitzpatrick} - {perla}");
        }
    }

    // Test scars generator
    [Fact]
    public void TestScarsGenerator()
    {
        var location = ScarsGenerator.GenerateLocation();
        Assert.InRange(location.Length, 8, 50);

        var size = ScarsGenerator.GenerateSize();
        Assert.True(size > 0);

        var filePath = ScarsGenerator.GeneratePhoto();
        Assert.True(File.Exists(filePath));
        testOutputHelper.WriteLine($"{location} - {size} - {filePath}");
    }

    // Test weight generator
    [Fact]
    public void TestWeightGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var weight = WeightGenerator.GenerateWeight();
            Assert.InRange(weight, 60, 160);

            testOutputHelper.WriteLine(weight.ToString(CultureInfo.CurrentCulture));
        }
    }
}