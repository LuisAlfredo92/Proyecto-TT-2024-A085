﻿namespace Identifying_data.Localities;

public class LocalitiesGenerator
{
    public static readonly string[] Localities = File.ReadAllLines("Localities/Localities.txt");

    public static string Generate() => Localities[Random.Shared.Next(Localities.Length)];
}