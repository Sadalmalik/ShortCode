using System;
using UnityEngine;

namespace GearBoxTools
{
    public enum RectTransformComponent
    {
        AnchoredPosition,
        AnchorMin,
        AnchorMax,
        OffsetMin,
        OffsetMax,
        Pivot,
        SizeDelta,
    }
    
    public static class RectTransformExtensions
    {
        public static Vector2 GetRectVector(this RectTransform rect, RectTransformComponent rectComponent)
        {
            switch (rectComponent)
            {
                case RectTransformComponent.AnchoredPosition:
                    return rect.anchoredPosition;
                case RectTransformComponent.AnchorMin:
                    return rect.anchorMin;
                case RectTransformComponent.AnchorMax:
                    return rect.anchorMax;
                case RectTransformComponent.OffsetMin:
                    return rect.offsetMin;
                case RectTransformComponent.OffsetMax:
                    return rect.offsetMax;
                case RectTransformComponent.Pivot:
                    return rect.pivot;
                case RectTransformComponent.SizeDelta:
                    return rect.sizeDelta;
            }
            
            throw new Exception($"Unknown rect transform component: {rectComponent}");
        }
        
        public static void SetRectVector(this RectTransform rect, RectTransformComponent rectComponent, Vector2 vector)
        {
            switch (rectComponent)
            {
                case RectTransformComponent.AnchoredPosition:
                    rect.anchoredPosition = vector; return;
                case RectTransformComponent.AnchorMin:
                    rect.anchorMin = vector; return;
                case RectTransformComponent.AnchorMax:
                    rect.anchorMax = vector; return;
                case RectTransformComponent.OffsetMin:
                    rect.offsetMin = vector; return;
                case RectTransformComponent.OffsetMax:
                    rect.offsetMax = vector; return;
                case RectTransformComponent.Pivot:
                    rect.pivot = vector; return;
                case RectTransformComponent.SizeDelta:
                    rect.sizeDelta = vector; return;
            }
            
            throw new Exception($"Unknown rect transform component: {rectComponent}");
        }
    }
}