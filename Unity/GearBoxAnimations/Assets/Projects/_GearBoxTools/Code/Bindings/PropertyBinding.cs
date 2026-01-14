using System;
using UnityEngine;

namespace GearBoxTools
{
    [Serializable]
    public abstract class PropertyBinding<T>
    {
        public abstract void SetupTarget(GameObject target);
        public abstract T GetValue();
        public abstract void SetValue(T value);
    }
}