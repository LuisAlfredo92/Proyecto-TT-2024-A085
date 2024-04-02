namespace General_Data
{
    public class DatesGenerator
    {
        public static DateTime GenerateDate(DateTime start, DateTime end)
        {
            var range = (end - start).Days;
            return start.AddDays(Random.Shared.Next(range));
        }
    }
}
