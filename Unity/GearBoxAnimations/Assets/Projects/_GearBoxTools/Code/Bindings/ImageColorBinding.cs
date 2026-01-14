using System;
using UnityEngine;
using UnityEngine.UI;

namespace GearBoxTools
{
    [Serializable]
    public class ImageColorBinding : PropertyBinding<Color>
    {
        public Image image;
        public ColorChannel channel;

        public override void SetupTarget(GameObject target)
        {
            image = target.GetComponent<Image>();
        }

        public override Color GetValue()
        {
            return image.color;
        }

        public override void SetValue(Color value)
        {
            image.color = value;
        }
    }
}