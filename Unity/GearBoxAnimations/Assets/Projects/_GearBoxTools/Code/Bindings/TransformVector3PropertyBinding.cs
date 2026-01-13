using System;
using UnityEngine;

namespace GearBoxTools
{
    [Serializable]
    public class TransformVector3PropertyBinding : PropertyBinding<Vector3>
    {
        public Transform target;

        public TransformComponent component;
        
        public override Vector3 GetValue()
        {
            return target.GetVector(component);
        }

        public override void SetValue(Vector3 value)
        {
            target.SetVector(component, value);
        }
    }
}