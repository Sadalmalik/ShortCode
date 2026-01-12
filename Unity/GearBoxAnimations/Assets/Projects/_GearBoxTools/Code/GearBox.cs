using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GearBoxTools
{
    public enum GearType
    {
        GearRatio,
        NormalizedCurve
    }

    public class Gear
    {
        public GearType GearType;

        [HorizontalGroup("Ratio")]
        [ShowIf(nameof(IsGearRation))]
        public int InGear = 1;

        [HorizontalGroup("Ratio")]
        [ShowIf(nameof(IsGearRation))]
        public int OutGear = 1;

        [ShowIf(nameof(IsNormalizedCurve))]
        public AnimationCurve Curve;
        [ShowIf(nameof(IsNormalizedCurve))]
        public float CurveScale = 1;

        public bool IsGearRation => GearType == GearType.GearRatio;
        public bool IsNormalizedCurve => GearType == GearType.NormalizedCurve;

        public PropertyBinding outputProperty;

        public void ApplyValue(float value)
        {
            var outValue = value;

            switch (GearType)
            {
                case GearType.GearRatio:
                    outValue = -(value * InGear) / OutGear;
                    break;
                case GearType.NormalizedCurve:
                    // Expect value be normalized
                    outValue = CurveScale * Curve.Evaluate(Mathf.Clamp01(value));
                    break;
            }

            outputProperty.SetValue(outValue);
        }
    }

    [ExecuteInEditMode]
    public class GearBox : SerializedMonoBehaviour
    {
        public PropertyBinding inputProperty;
        public float Offset;
        public float Scale;
        public List<Gear> gears;

        public bool ExecuteInEditor = false;

        private void Update()
        {
            if (!ExecuteInEditor) return;

            var value = Scale * (inputProperty.GetValue() + Offset);
            foreach (var gear in gears)
            {
                gear.ApplyValue(value);
            }
        }
    }
}