using Sirenix.OdinInspector;
using UnityEngine;

namespace GearBoxTools
{
    [ExecuteInEditMode]
    public class InverseRotator : SerializedMonoBehaviour
    {
        private void Update()
        {
            if (transform.parent == null)
                return;
            transform.localRotation = Quaternion.Inverse(transform.parent.localRotation);
        }
    }
}