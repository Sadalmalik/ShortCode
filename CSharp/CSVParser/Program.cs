using System;
using Sadalmalik.CSV;

namespace Sadalmalik
{
    public class CharacterData
    {
        public int id;
        public string name;
        public string description;
        public int power;
    }

    internal class Application
    {
        private static string CharactersURL =
            "https://docs.google.com/spreadsheets/d/e/2PACX-1vR79p8WJLvL-29paJ_rzm1RXP99q4n4xlTrBJ-UE_KFJ5A_w8pTRFXZjBEPL6QO5EwE_CeuNyJLKxer/pub?gid=1419529400&single=true&output=csv";
        
        static async Task Main(string[] args)
        {
            TestCSV("Normal CSV test", "Samples/sample-1.csv");
            TestCSV("Escaped CSV test", "Samples/sample-2.csv");
            TestCSV("Broken CSV test", "Samples/sample-3.csv");
            await TestCharacters();
        }

        private static void TestCSV(string title, string filePath)
        {
            var oldCSV = File.ReadAllText(filePath);

            var values = CSVParser.ReadFromString(oldCSV).ToList();
            var newCSV = CSVParser.WriteToString(values);

            Console.WriteLine(title);
            Console.WriteLine($"Original CSV:\n{oldCSV}\n");

            Console.WriteLine($"Parsed values:");
            foreach (var value in values)
                Console.WriteLine($"\t{value}");

            Console.WriteLine($"Generated CSV:\n{newCSV}\n");
        }

        private static async Task TestCharacters()
        {
            var csv = await LoadCSVFromURL(CharactersURL);
            if (csv == null)
            {
                Console.WriteLine("No remote data found! Use local fallback");
                csv = await File.ReadAllTextAsync("Samples/characters.csv");
            }
            var characters = CSVUtils.FromCSV<CharacterData>(csv);

            foreach (var character in characters)
            {
                Console.WriteLine($"Character #{character.id}");
                Console.WriteLine($"  Name:  {character.name}");
                Console.WriteLine($"  Power: {character.power}");
                Console.WriteLine("  Description:");
                Console.WriteLine(character.description);
                Console.WriteLine();
            }
        }

        private static async Task<string?> LoadCSVFromURL(string url)
        {
            using var client = new HttpClient();
            
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}























