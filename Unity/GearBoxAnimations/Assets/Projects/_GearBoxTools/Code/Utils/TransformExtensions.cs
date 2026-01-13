using System;
using UnityEngine;

namespace GearBoxTools
{
    public enum VectorAxis
    {
        X=0, Y=1, Z=2, W=3
    }
    
    public enum TransformComponent
    {
        Position,
        Rotation,
        LossyScale,
        LocalPosition,
        LocalRotation,
        LocalScale,
    }
    
    public static class TransformExtensions
    {
        public static Vector3 GetVector(this Transform transform, TransformComponent component)
        {
            switch (component)
            {
                case TransformComponent.Position:
                    return transform.position;
                case TransformComponent.Rotation:
                    return transform.rotation.eulerAngles;
                case TransformComponent.LossyScale:
                    return transform.lossyScale;
                case TransformComponent.LocalPosition:
                    return transform.localPosition;
                case TransformComponent.LocalRotation:
                    return transform.localRotation.eulerAngles;
                case TransformComponent.LocalScale:
                    return transform.localScale;
            }
            throw new Exception($"Unknown transform component: {component}");
        }
        
        public static void SetVector(this Transform transform, TransformComponent component, Vector3 vector)
        {
            switch (component)
            {
                case TransformComponent.Position:
                    transform.position = vector;
                    return;
                case TransformComponent.Rotation:
                    transform.rotation = Quaternion.Euler(vector);
                    return;
                case TransformComponent.LossyScale:
                    throw new Exception("Lossy Scale can't be set!");
                case TransformComponent.LocalPosition:
                    transform.localPosition = vector;
                    return;
                case TransformComponent.LocalRotation:
                    transform.localRotation = Quaternion.Euler(vector);
                    return;
                case TransformComponent.LocalScale:
                    transform.localScale = vector;
                    return;
            }
            throw new Exception($"Unknown transform component: {component}");
        }
    }
}