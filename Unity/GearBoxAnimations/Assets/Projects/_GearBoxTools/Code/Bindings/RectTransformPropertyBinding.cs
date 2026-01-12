using Sirenix.OdinInspector;
using UnityEngine;

namespace GearBoxTools
{
    public class RectTransformPropertyBinding : PropertyBinding
    {
        public RectTransform target;
        
        [HorizontalGroup]
        public RectTransformComponent component;
        
        [HorizontalGroup]
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