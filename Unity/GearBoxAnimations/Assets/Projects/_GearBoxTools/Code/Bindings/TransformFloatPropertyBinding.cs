using System;
using UnityEngine;

namespace GearBoxTools
{
    [Serializable]
    public class TransformFloatPropertyBinding : PropertyBinding<float>
    {
        public Transform target;

        public TransformComponent component;
        
        public VectorAxis axis;
        
        public override float GetValue()
        {
            var vector = target.GetVector(component);
            return vector[(int)axis];
        }

        public override void SetValue(float value)
        {
            var vector = target.GetVector(component);
            vector[(int)axis] = value;
            target.SetVector(component, vector);
        }
    }
}