using System.Text;

namespace Sadalmalik.CSV
{
    public static class CSVParser
    {
        public static List<List<string>> ReadTable(string csv, Encoding? encoding = null)
        {
            return ReadLinesFromString(csv, encoding).ToList();
        }

        public static string WriteTable(List<List<string>> table, Encoding? encoding = null)
        {
            return WriteLinesToString(table, encoding);
        }

        public static IEnumerable<List<string>> ReadLinesFromString(string csv, Encoding? encoding = null)
        {
            return ReadFromString(csv, encoding).ToLines();
        }

        public static string WriteLinesToString(IEnumerable<List<string>> lines, Encoding? encoding = null)
        {
            return WriteToString(lines.ToValues(), encoding);
        }

        public static IEnumerable<string?> ReadFromString(string csv, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var bytes = encoding.GetBytes(csv);
            var stream = new MemoryStream(bytes);
            var reader = new StreamReader(stream, encoding);
            return Read(reader, stream, reader);
        }

        public static string WriteToString(IEnumerable<string?> values, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, encoding);
            Write(writer, values);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }

        public static IEnumerable<string?> Read(StreamReader reader, params IDisposable[] disposables)
        {
            var quotedString = false;
            var buffer = new StringBuilder();

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
                        yield return buffer.ToString();
                        buffer.Clear();
                        yield return null;
                        if (curr == '\r' && next == '\n')
                            reader.Read();
                        continue;
                    }

                    if (curr == ',')
                    {
                        yield return buffer.ToString();
                        buffer.Clear();
                        continue;
                    }

                    if (curr == '"')
                    {
                        quotedString = true;
                        continue;
                    }
                }

                buffer.Append(curr);
            }

            yield return buffer.ToString();
            buffer.Clear();
            yield return null;

            foreach (var disposable in disposables)
                disposable.Dispose();
        }

        public static void Write(StreamWriter writer, IEnumerable<string?> values)
        {
            var needSeparator = false;
            foreach (var value in values)
            {
                if (value == null)
                {
                    writer.WriteLine();
                    needSeparator = false;
                    continue;
                }

                if (needSeparator)
                    writer.Write(',');
                needSeparator = true;

                if (value.Contains('"') ||
                    value.Contains(',') ||
                    value.Contains('\n') ||
                    value.Contains('\r'))
                {
                    writer.Write('"');
                    writer.Write(value.Replace("\"", "\"\""));
                    writer.Write('"');
                    continue;
                }

                writer.Write(value);
            }
        }
    }
}