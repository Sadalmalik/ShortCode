using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDriver : MonoBehaviour
{
    public List<RectTransform> Layouts;

    private void Update()
    {
        foreach (var layout in Layouts)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
        }
    }
}
