using UnityEngine;

namespace GearBoxTools
{
    public class BouncyScale2D : MonoBehaviour
    {
        public RectTransform ControllableRect;

        [Space]
        public SpringSetting scaleSpring;

        public Spring xScaleSpring;
        public Spring yScaleSpring;

        [Space]
        public Vector2 targetPosition;

        public Vector2 targetScale;

        [Space]
        public bool BounceInOnStart;

        public float BouncePower;

        [Space]
        public bool TickBumpOnStart;

        public float BumpDelay;
        public float BumpPower;

        private bool m_BumpEnabled;
        private float m_NextTime;

        private void Awake()
        {
            SetBump(TickBumpOnStart);
            targetPosition = ControllableRect.anchoredPosition;
            targetScale = ControllableRect.localScale;
            xScaleSpring.value = ControllableRect.localScale.x;
            yScaleSpring.value = ControllableRect.localScale.y;
            if (BounceInOnStart)
                BounceIn();
        }

        private void FixedUpdate()
        {
            if (m_BumpEnabled && m_NextTime <= Time.time)
            {
                m_NextTime += BumpDelay;
                BumpScale(BumpPower);
            }

            var dt = Time.fixedDeltaTime;
            scaleSpring.Tick(dt, targetScale.x, ref xScaleSpring);
            scaleSpring.Tick(dt, targetScale.y, ref yScaleSpring);

            ControllableRect.localScale = new Vector3(xScaleSpring.value, yScaleSpring.value);
        }

        public void SetBump(bool enable)
        {
            if (m_BumpEnabled != enable && enable)
                m_NextTime = Time.time + BumpDelay;
            m_BumpEnabled = enable;
        }

        public void BumpScale(float power)
        {
            xScaleSpring.velocity += 2 * power;
            yScaleSpring.velocity -= 2 * power;
        }

        public void BounceIn()
        {
            SetupScale(Vector2.zero, new Vector2(2, 1) * BouncePower, Vector2.one);
        }

        public void BounceOut()
        {
            SetupScale(Vector2.one, new Vector2(2, 1) * BouncePower, Vector2.zero);
        }

        public void SetupScale(Vector2 scale, Vector2 velocity, Vector2 target)
        {
            xScaleSpring.value = scale.x;
            yScaleSpring.value = scale.y;
            xScaleSpring.velocity = velocity.x;
            yScaleSpring.velocity = velocity.y;
            targetScale = target;
        }
    }
}