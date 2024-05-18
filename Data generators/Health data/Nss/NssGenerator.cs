namespace Health_data.Nss;

public class NssGenerator
{
    public static long Generate() => Random.Shared.NextInt64(10_000_000_000, 99_999_999_999);
}