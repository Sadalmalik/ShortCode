using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Mode
{
    Sequential = 0,
    Parallel = 1,
}

public enum HandleMode
{
    AnchorPos,
    SizeDelta
}

[Serializable]
public class Handle
{
    public RectTransform handle;
    public HandleMode mode;
    public Vector2 endValue;
    public float duration;
    
    public Tween DoTween()
    {
        switch (mode)
        {
            case HandleMode.AnchorPos:
                return handle.DOAnchorPos(endValue, duration).SetEase(Ease.InOutQuad);
            case HandleMode.SizeDelta:
                return handle.DOSizeDelta(endValue, duration).SetEase(Ease.InOutQuad);
        }

        return null;
    }
}

[Serializable]
public class HandlesGroup
{
    public Mode mode=Mode.Parallel;
    public List<Handle> Handles;
    public float endPause;
    
    public float DoTween(Sequence sequence, float time)
    {
        switch (mode)
        {
            case Mode.Sequential:
                foreach (var handle in Handles)
                {
                    sequence.Insert(time, handle.DoTween());
                    time += handle.duration;
                }
                break;
            case Mode.Parallel:
                var max = 0f;
                foreach (var handle in Handles)
                {
                    sequence.Insert(time, handle.DoTween());
                    max = Mathf.Max(max, handle.duration);
                }
                time += max;
                break;
        }

        return time + endPause;
    }
}

public class SceneDriver : MonoBehaviour
{
    public List<RectTransform> Layouts;

    [Space]
    public float inPause = 2f;
    public List<HandlesGroup> Groups;

    [Space]
    public List<RectTransform> targets;
    public float duration = 1f;
    public float delay = 0.1f;
    public Vector2 targetSizeDelta = new Vector2(350, 0);
    
    private void Start()
    {
        var index = 0;
        var sequence = DOTween.Sequence();
        var startTime = inPause;
        
        foreach (var group in Groups)
        {
            startTime = group.DoTween(sequence, startTime);
        }
        
        // Do rest splits
        foreach (var target in targets)
        {
            sequence.Insert(
                startTime + index * delay,
                target.DOSizeDelta(targetSizeDelta, duration).SetEase(Ease.InOutQuad));
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
