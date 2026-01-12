using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GearBoxTools
{
    [Serializable]
    public class SpringSetting
    {
        public float springStrength = 150f;
        public float springDamping = 10f;
        
        public void Tick(float deltaTime, float target, ref Spring spring)
        {
            var delta = target - spring.value;
            var force = delta * springStrength - spring.velocity * springDamping;
            spring.velocity += deltaTime * force;
            spring.value += deltaTime * spring.velocity;
        }
    }

    [Serializable]
    public class Spring
    {
        public float value;
        public float velocity;
    }

    public class Spring2D : MonoBehaviour
    {
        public SpringSetting spring;
        public Spring xSpring;
        public Spring ySpring;
        public RectTransform targetRect;
        public RectTransform sourceRect;
        public float bumpForce;
        public float delay;

        private float m_NextTime;

        private void Awake()
        {
            xSpring.value = targetRect.anchoredPosition.x;
            ySpring.value = targetRect.anchoredPosition.y;
        }

        private void FixedUpdate()
        {
            if (m_NextTime < Time.time)
            {
                m_NextTime = Time.time + delay;
                var dir = Random.insideUnitCircle.normalized * bumpForce;
                xSpring.velocity += dir.x;
                ySpring.velocity += dir.y;
            }

            var dt = Time.fixedDeltaTime;
            spring.Tick(dt, sourceRect.anchoredPosition.x, ref xSpring);
            spring.Tick(dt, sourceRect.anchoredPosition.y, ref ySpring);

            targetRect.anchoredPosition = new Vector2(xSpring.value, ySpring.value);
        }
    }
}