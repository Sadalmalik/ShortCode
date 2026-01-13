using System;
using UnityEngine;

namespace GearBoxTools
{
    [Serializable]
    public class RectTransformVector2PropertyBinding : PropertyBinding<Vector2>
    {
        public RectTransform target;
        public RectTransformComponent component;
        
        public override Vector2 GetValue()
        {
            return target.GetRectVector(component);
        }

        public override void SetValue(Vector2 value)
        {
            target.SetRectVector(component, value);
        }
    }
}