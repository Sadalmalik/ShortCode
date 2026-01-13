using System;
using UnityEngine;

namespace GearBoxTools
{
    [Serializable]
    public class RectTransformFloatPropertyBinding : PropertyBinding<float>
    {
        public RectTransform target;
        public RectTransformComponent component;
        public VectorAxis axis;
        
        public override float GetValue()
        {
            var vector = target.GetRectVector(component);
            return vector[(int)axis];
        }

        public override void SetValue(float value)
        {
            var vector = target.GetRectVector(component);
            vector[(int)axis] = value;
            target.SetRectVector(component, vector);
        }
    }
}