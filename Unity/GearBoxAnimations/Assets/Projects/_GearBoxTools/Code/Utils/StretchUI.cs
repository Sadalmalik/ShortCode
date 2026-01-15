using UnityEngine;

namespace GearBoxTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class StretchUI : MonoBehaviour
    {
        public RectTransform from;
        public RectTransform into;
        public float height = 100;
        
        private void Update()
        {
            if (from == null) return;
            if (into == null) return;
            
            var a = from.anchoredPosition;
            var b = into.anchoredPosition;
            var delta = b - a;
            var dist = delta.magnitude;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            delta.Normalize();
            var middle = (a + b) * 0.5f;

            var rect = transform as RectTransform;
            if (rect == null) return;
            rect.anchoredPosition = middle;
            rect.sizeDelta = new Vector2(dist, height);
            rect.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}