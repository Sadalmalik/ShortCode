using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;

namespace GearBoxTools
{
    public class FloatPropertyBinding<T> : PropertyBinding
    {
        public T target;
        
        [ValueDropdown("GetFields")]
        public FieldInfo field;

        public override float GetValue()
        {
            return (float) field.GetValue(target);
        }

        public override void SetValue(float value)
        {
            field.SetValue(target, value);
        }
        
        private List<FieldInfo> GetFields()
        {
            return typeof(T)
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fieldInfo=>fieldInfo.FieldType==typeof(float))
                .ToList();
        }
    }
}