namespace Sadalmalik.CSV
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<List<string>> ToLines(this IEnumerable<string?> values)
        {
            var line = new List<string>();

            foreach (var value in values)
            {
                if (value == null)
                {
                    yield return line;
                    line = new List<string>();
                    continue;
                }
                
                line.Add(value);
            }
        }

        public static IEnumerable<string?> ToValues(this IEnumerable<List<string>> lines)
        {
            foreach (var line in lines)
            {
                foreach (var value in line)
                    yield return value;
                yield return null;
            }
        }
    }
}