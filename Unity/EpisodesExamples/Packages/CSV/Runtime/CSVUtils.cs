using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sadalmalik.CSV
{
    public class CSVUtils
    {
        public static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static readonly string[] TrueStatements = {"true", "yes", "истина", "да"};
        
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
            for (int i = 0; i < header.Count; i++)
            {
                var name = header[i];
                if (name.StartsWith('#')) continue;
                fields.Add(i, type.GetField(name, flags));
            }

            while (enumerator.MoveNext())
            {
                var line = enumerator.Current;

                var item = new T();
                for (int i = 0; i < line.Count; i++)
                {
                    if (!fields.TryGetValue(i, out var field))
                        continue;
                    if (field==null)
                        continue;
                    
                    var value = FromString(field.FieldType, line[i]);
                    
                    field.SetValue(item, value);
                }
                items.Add(item);
            }

            return items;
        }

        public static string ToCSV<T>(List<T> items, bool addPrimaryIndex=false)
        {
            return CSVParser.WriteToString(ToValues(items, addPrimaryIndex));
        }

        private static IEnumerable<string?> ToValues<T>(List<T> items, bool addPrimaryIndex=false)
        {
            if (addPrimaryIndex)
                yield return "#";
            var fields = typeof(T).GetFields(flags);
            foreach (var field in fields)
                yield return field.Name;
            yield return null;

            var index = 0;
            foreach (var item in items)
            {
                if (addPrimaryIndex)
                {
                    yield return index.ToString();
                    index++;
                }
                
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
                return value as string;

            if (type == typeof(bool))
            {
                return TrueStatements.Contains(value);
            }
                
            if (type == typeof(byte))
            {
                if (byte.TryParse(value, out var result))
                    return result;
                return (byte) 0;
            }

            if (type == typeof(sbyte))
            {
                if (sbyte.TryParse(value, out var result))
                    return result;
                return (sbyte) 0;
            }

            if (type == typeof(short))
            {
                if (short.TryParse(value, out var result))
                    return result;
                return (short) 0;
            }

            if (type == typeof(int))
            {
                if (int.TryParse(value, out var result))
                    return result;
                return (int) 0;
            }

            if (type == typeof(long))
            {
                if (long.TryParse(value, out var result))
                    return result;
                return (long) 0;
            }

            if (type == typeof(float))
            {
                if (float.TryParse(value, out var result))
                    return result;
                return (float) 0;
            }

            if (type == typeof(double))
            {
                if (double.TryParse(value, out var result))
                    return result;
                return (double) 0;
            }
            
            return value;
        }

        private static string ToString(Type type, object value)
        {
            if (type == typeof(bool))
                return (bool)value ? "true" : "false";
                
            return value.ToString() ?? "";
        }
    }
}





















