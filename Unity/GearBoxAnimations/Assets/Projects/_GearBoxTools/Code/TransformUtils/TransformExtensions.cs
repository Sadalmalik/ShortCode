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
        Scale
    }
    
    public static class TransformExtensions
    {
        public static Vector3 GetVector(this Transform transform, TransformComponent component, bool local)
        {
            switch (component)
            {
                case TransformComponent.Position:
                    return local ? transform.localPosition : transform.position;
                case TransformComponent.Rotation:
                    return local ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;
                case TransformComponent.Scale:
                    return local ? transform.localScale : transform.lossyScale;
            }
            throw new Exception($"Unknown transform component: {component}");
        }
        
        public static void SetVector(this Transform transform, TransformComponent component, bool local, Vector3 vector)
        {
            switch (component)
            {
                case TransformComponent.Position:
                    if (local) transform.localPosition = vector;
                    else transform.position = vector;
                    return;
                case TransformComponent.Rotation:
                    if (local) transform.localRotation = Quaternion.Euler(vector);
                    else transform.rotation = Quaternion.Euler(vector);
                    return;
                case TransformComponent.Scale:
                    if (local) transform.localScale = vector;
                    else throw new Exception("Lossy Scale can't be set!");
                    return;
            }
            throw new Exception($"Unknown transform component: {component}");
        }
    }
}