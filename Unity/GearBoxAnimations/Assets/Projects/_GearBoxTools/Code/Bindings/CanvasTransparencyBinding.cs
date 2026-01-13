using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GearBoxTools
{
    [Serializable]
    public class CanvasTransparencyBinding : PropertyBinding<float>
    {
        [FormerlySerializedAs("Group")]
        public CanvasGroup group;
        
        public override float GetValue()
        {
            return group.alpha;
        }

        public override void SetValue(float value)
        {
            group.alpha = value;
        }
    }
}