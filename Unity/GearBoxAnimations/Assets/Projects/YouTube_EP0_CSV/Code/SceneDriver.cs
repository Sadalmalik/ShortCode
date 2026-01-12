using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneDriver : MonoBehaviour
{
    public List<RectTransform> Layouts;
    
    public List<RectTransform> targets;
    public List<TMP_Text> pops;
    public float inPause = 2f;
    public float duration = 1f;
    public float delay = 0.1f;
    public float popDelay = 0.1f;
    public Vector2 targetSizeDelta = new Vector2(250, 0);
    
    private void Start()
    {
        var index = 0;
        var sequence = DOTween.Sequence();
        
        foreach (var target in targets)
        {
            sequence.Insert(
                inPause + index * delay,
                target.DOSizeDelta(targetSizeDelta, duration));
            index++;
        }

        index = 0;
        foreach (var pop in pops)
        {
            sequence.Insert(
                inPause + index * delay + popDelay,
                pop.DOFade(1, duration * 0.01f));
            sequence.Insert(
                inPause + index * delay + popDelay + duration * 0.25f,
                pop.DOFade(0, duration * 0.15f));
            index++;
        }
    }

    private void Update()
    {
        foreach (var layout in Layouts)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
        }
    }
}
