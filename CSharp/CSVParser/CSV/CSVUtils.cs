using System.Reflection;

namespace Sadalmalik.CSV
{
    public static class CSVUtils
    {
        private static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly string[] TrueStatements = ["true", "yes", "истина", "да"];

        public static List<T> FromCSV<T>(string csv) where T : new()
        {
            var type = typeof(T);
            var items = new List<T>();

            var enumerable = CSVParser.ReadLinesFromString(csv);
            using var enumerator = enumerable.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                // Stream ended before header line!
                return items;
            }

            var header = enumerator.Current;
            var fields = new Dictionary<int, FieldInfo?>();
            for (var i = 0; i < header.Count; i++)
            {
                var name = header[i];
                if (name.StartsWith("#")) continue;
                fields.Add(i, type.GetField(name, flags));
            }

            while (enumerator.MoveNext())
            {
                var line = enumerator.Current;

                var item = new T();
                for (var k = 0; k < line.Count; k++)
                {
                    if (!fields.TryGetValue(k, out var field))
                        continue;
                    if (field == null)
                        continue;

                    var value = FromString(field.FieldType, line[k]);

                    field.SetValue(item, value);
                }

                items.Add(item);
            }

            return items;
        }

        public static string ToCSV<T>(List<T> items, bool addPrimaryIndex = true)
        {
            return CSVParser.WriteToString(ToValues(items, addPrimaryIndex));
        }

        private static IEnumerable<string?> ToValues<T>(List<T> items, bool addPrimaryIndex = true)
        {
            if (addPrimaryIndex)
                yield return "#";
            var fields = typeof(T).GetFields(flags);
            foreach (var field in fields)
                yield return field.Name;

            yield return null;

            int index = 0;
            foreach (var item in items)
            {
                if (addPrimaryIndex)
                    yield return index.ToString();

                foreach (var field in fields)
                {
                    var value = field.GetValue(item)!;
                    yield return ToString(field.FieldType, value);
                }

                yield return null;
            }
        }

        private static object FromString(Type type, string value)
        {
            if (type == typeof(string))
                return value;

            if (type == typeof(bool))
                return TrueStatements.Contains(value);
            if (type == typeof(byte))
            {
                if (byte.TryParse(value, out byte result))
                    return result;
                return (byte)0;
            }

            if (type == typeof(short))
            {
                if (short.TryParse(value, out short result))
                    return result;
                return (short)0;
            }

            if (type == typeof(int))
            {
                if (int.TryParse(value, out int result))
                    return result;
                return (int)0;
            }

            if (type == typeof(long))
            {
                if (long.TryParse(value, out long result))
                    return result;
                return (long)0;
            }

            if (type == typeof(float))
            {
                if (float.TryParse(value, out float result))
                    return result;
                return (float)0;
            }

            if (type == typeof(double))
            {
                if (double.TryParse(value, out double result))
                    return result;
                return (double)0;
            }

            return value;
        }

        private static string ToString(Type type, object value)
        {
            if (type == typeof(string))
                return value.ToString();

            if (type == typeof(bool))
                return (bool)value ? "true" : "false";

            return value.ToString();
        }
    }
}