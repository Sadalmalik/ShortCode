using UnityEngine;

namespace GearBoxTools
{
    public class CanvasBinding : PropertyBinding
    {
        public CanvasGroup Group;
        
        public override float GetValue()
        {
            return Group.alpha;
        }

        public override void SetValue(float value)
        {
            Group.alpha = value;
        }
    }
}