using System;

namespace GearBoxTools
{
    [Serializable]
    public class Spring
    {
        public float value;
        public float velocity;
    }
    
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
}