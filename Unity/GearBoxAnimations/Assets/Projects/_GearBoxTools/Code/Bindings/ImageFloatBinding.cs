using System;
using UnityEngine.UI;

namespace GearBoxTools
{
    [Serializable]
    public class ImageFloatBinding : PropertyBinding<float>
    {
        public Image image;
        public ColorChannel channel;
        
        public override float GetValue()
        {
            return image.color[(int) channel];
        }

        public override void SetValue(float value)
        {
            var color = image.color;
            color[(int)channel] = value;
            image.color = color;
        }
    }
}