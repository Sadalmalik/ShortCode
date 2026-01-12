using Sirenix.OdinInspector;
using UnityEngine;

namespace GearBoxTools
{
    public class TransformPropertyBinding : PropertyBinding
    {
        public Transform target;

        [HorizontalGroup]
        public bool useLocal;

        [HorizontalGroup]
        public TransformComponent component;
        
        [HorizontalGroup]
        public VectorAxis axis;
        
        public override float GetValue()
        {
            var vector = target.GetVector(component, useLocal);
            return vector[(int)axis];
        }

        public override void SetValue(float value)
        {
            var vector = target.GetVector(component, useLocal);
            vector[(int)axis] = value;
            target.SetVector(component, useLocal, vector);
        }
    }
}