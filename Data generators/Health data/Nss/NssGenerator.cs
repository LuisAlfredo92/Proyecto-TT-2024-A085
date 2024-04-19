namespace Health_data.Nss;

public class NssGenerator
{
    public static long Generate() => Random.Shared.NextInt64(10000000000, 99999999999);
}