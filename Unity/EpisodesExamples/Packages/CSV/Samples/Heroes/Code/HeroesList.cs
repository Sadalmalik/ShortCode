using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sadalmalik.CSV;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public string description;
    public int power;
}

[CreateAssetMenu(fileName = "HeroesList", menuName = "Scriptable Objects/HeroesList")]
public class HeroesList : ScriptableObject
{
    public string SourceURL;
    public List<CharacterData> Characters;

    public async Task Load()
    {
        var client = new HttpClient();
        var csv = await client.GetStringAsync(SourceURL);

        Characters = CSVUtils.FromCSV<CharacterData>(csv);
    }
}
