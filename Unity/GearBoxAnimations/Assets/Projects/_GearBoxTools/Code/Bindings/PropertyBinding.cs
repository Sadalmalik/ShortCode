using System;

namespace GearBoxTools
{
    [Serializable]
    public abstract class PropertyBinding<T>
    {
        public abstract T GetValue();
        public abstract void SetValue(T value);
    }
}