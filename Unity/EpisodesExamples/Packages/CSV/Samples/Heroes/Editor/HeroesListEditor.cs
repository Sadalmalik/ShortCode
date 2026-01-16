using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeroesList))]
public class HeroesListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Load"))
        {
            var config = target as HeroesList;
            if (config != null)
                _ = config.Load();
        }
        DrawDefaultInspector();
    }
}
