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
    
    internal class Program
    {
        private static readonly string CharactersURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vR79p8WJLvL-29paJ_rzm1RXP99q4n4xlTrBJ-UE_KFJ5A_w8pTRFXZjBEPL6QO5EwE_CeuNyJLKxer/pub?gid=1419529400&single=true&output=csv";
        
        static async Task Main(string[] args)
        {
            TestCSV("Normal CSV test", "Samples/sample-1.csv");
            TestCSV("Escaped CSV test", "Samples/sample-2.csv");
            TestCSV("Broken CSV test", "Samples/sample-3.csv");

            await TestCharactersParsing();
        }

        private static void TestCSV(string title, string filepath)
        {
            var oldCSV = File.ReadAllText(filepath);
            var values = CSVParser.ReadFromString(oldCSV).ToList();
            var newCSV = CSVParser.WriteToString(values);
            
            Console.WriteLine(title);
            Console.WriteLine($"Original CSV:\n{oldCSV}\n");
            Console.WriteLine($"Generated CSV:\n{newCSV}\n");
            
            Console.WriteLine("Parsed values:");
            foreach (var value in values)
                Console.WriteLine($"\t{value}");
        }

        private static async Task TestCharactersParsing()
        {
            //var csv = File.ReadAllText("Samples/Samples - Heroes.csv");

            Console.WriteLine($"Downloading: {CharactersURL}");
            using var client = new HttpClient();
            var csv = await client.GetStringAsync(CharactersURL);
            var characters = CSVUtils.FromCSV<CharacterData>(csv);

            foreach (var character in characters)
            {
                Console.WriteLine($"Character #{character.id}");
                Console.WriteLine($"  Name: {character.name}");
                Console.WriteLine($"  Power: {character.power}");
                Console.WriteLine($"Description:\n{character.description}");
                Console.WriteLine();
            }

            var outputCsv = CSVUtils.ToCSV(characters);
            Console.Write($"Generated CSV:\n{outputCsv}\n\n\n");
        }
    }
}























