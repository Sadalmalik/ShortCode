using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sadalmalik.CSV
{
    public static class CSVParser
    {
        public static IEnumerable<List<string>> ReadLinesFromString([NotNull] string csv, Encoding? encoding = null)
        {
            return ReadFromString(csv, encoding).ToLines();
        }

        public static string WriteLinesToString(IEnumerable<List<string>> lines, Encoding? encoding = null)
        {
            return WriteToString(lines.ToValues(), encoding);
        }

        public static IEnumerable<string?> ReadFromString([NotNull] string csv, Encoding? encoding = null)
        {
            var bytes = (encoding ?? Encoding.UTF8).GetBytes(csv);
            var stream = new MemoryStream(bytes);
            var reader = new StreamReader(stream);
            return Read(reader, reader, stream);
        }

        public static string WriteToString(IEnumerable<string?> values, Encoding? encoding = null)
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            Write(writer, values);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static IEnumerable<string?> Read(StreamReader reader, params IDisposable[] disposables)
        {
            var quotedString = false;
            var accumulator = new StringBuilder();

            while (!reader.EndOfStream)
            {
                var curr = (char)reader.Read();
                var next = (char)reader.Peek();

                if (quotedString)
                {
                    if (curr == '"')
                    {
                        if (next != '"')
                        {
                            quotedString = false;
                            continue;
                        }

                        reader.Read();
                    }
                }
                else
                {
                    if (curr == '\r' || curr == '\n')
                    {
                        yield return accumulator.ToString();
                        accumulator.Clear();
                        if (next == '\n')
                            reader.Read();
                        yield return null;
                        continue;
                    }

                    if (curr == ',')
                    {
                        yield return accumulator.ToString();
                        accumulator.Clear();
                        continue;
                    }

                    if (curr == '"')
                    {
                        quotedString = true;
                        continue;
                    }
                }

                accumulator.Append(curr);
            }

            yield return accumulator.ToString();
            accumulator.Clear();
            yield return null;

            foreach (var disposable in disposables)
                disposable.Dispose();
        }

        public static void Write(StreamWriter writer, IEnumerable<string?> values)
        {
            var needSeparator = false;
            foreach (var entry in values)
            {
                if (entry == null)
                {
                    writer.WriteLine();
                    needSeparator = false;
                    continue;
                }

                if (needSeparator)
                    writer.Write(',');
                needSeparator = true;

                if (entry.Contains('"') ||
                    entry.Contains(',') ||
                    entry.Contains('\n') ||
                    entry.Contains('\r'))
                {
                    writer.Write('"');
                    writer.Write(entry.Replace("\"", "\"\""));
                    writer.Write('"');
                    continue;
                }

                writer.Write(entry);
            }
        }
    }
}