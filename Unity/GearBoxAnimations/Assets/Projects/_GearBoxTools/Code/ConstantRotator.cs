using Sirenix.OdinInspector;
using UnityEngine;

namespace GearBoxTools
{
    public class ConstantRotator : SerializedMonoBehaviour
    {
        public Vector3 LocalAngle;
        public float Speed;
        
        private void Update()
        {
            transform.localRotation = Quaternion.Euler(LocalAngle * Speed * Time.time);
        }
    }
}